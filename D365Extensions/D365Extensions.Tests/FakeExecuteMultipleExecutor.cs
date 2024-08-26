using FakeXrmEasy;
using FakeXrmEasy.FakeMessageExecutors;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Tests
{
    /// <summary>
    /// To fake OOB behavior
    /// https://learn.microsoft.com/en-us/power-apps/developer/data-platform/org-service/execute-multiple-requests
    /// </summary>
    internal class FakeExecuteMultipleExecutor : IFakeMessageExecutor
    {
        private const string NameOfFaled = "IShouldFail";

        private const string ResponceIdKey = "ResponseId";

        public static OrganizationRequest GoodRequest => new OrganizationRequest() { RequestId = Guid.NewGuid() };

        public static OrganizationRequest FailRequest => new OrganizationRequest() { RequestId = Guid.NewGuid(), RequestName = NameOfFaled };
        
        public List<ExecuteMultipleRequest> ActualRequests { get; } = new List<ExecuteMultipleRequest>();

        public List<ExecuteMultipleResponse> Responses { get; } = new List<ExecuteMultipleResponse>();

        public static Guid GetRequestId(ExecuteMultipleResponseItem item)
        {
            if (item.IsFaulted())
            {
                return Guid.Parse(item.Fault.Message);
            }

            return (Guid)item.Response[ResponceIdKey];
        }

        public bool CanExecute(OrganizationRequest request)
        {
            return request is ExecuteMultipleRequest;
        }

        public bool IsFailed(OrganizationRequest orgRequest)
        {
            return orgRequest.RequestName == NameOfFaled;
        }

        public OrganizationResponse Execute(OrganizationRequest request, XrmFakedContext ctx)
        {
            var eMultipleRequest = request as ExecuteMultipleRequest;
            ActualRequests.Add(eMultipleRequest);

            var eMultipleResponce = new ExecuteMultipleResponse()
            {
                [nameof(ExecuteMultipleResponse.Responses)] = new ExecuteMultipleResponseItemCollection()
            };
            Responses.Add(eMultipleResponce);

            for (var i = 0; i < eMultipleRequest.Requests.Count; i++)
            {
                var orgRequest = eMultipleRequest.Requests[i];

                if (!IsFailed(orgRequest))
                {
                    //should return good responses
                    if (eMultipleRequest.Settings.ReturnResponses)
                    {
                        //add response to results
                        eMultipleResponce.Responses.Add(new ExecuteMultipleResponseItem
                        {
                            RequestIndex = i,
                            Response = new OrganizationResponse()
                            {
                                //to simplify request-responce mathing in a test
                                [ResponceIdKey] = orgRequest.RequestId
                            }
                        });
                    }
                }
                else
                {
                    //should always return errors
                    eMultipleResponce.Responses.Add(new ExecuteMultipleResponseItem
                    {
                        RequestIndex = i,
                        Fault = new OrganizationServiceFault()
                        {
                            //to simplify request-responce mathing in a test
                            Message = orgRequest.RequestId.ToString()
                        }
                    });

                    //should mark whole ExecuteMultipleResponse as faulted
                    eMultipleResponce[nameof(ExecuteMultipleResponse.IsFaulted)] = true;

                    // Should stop fake execution
                    if (!eMultipleRequest.Settings.ContinueOnError)
                        break;
                }
            }

            return eMultipleResponce;
        }

        public Type GetResponsibleRequestType()
        {
            return typeof(ExecuteMultipleRequest);
        }
    }
}
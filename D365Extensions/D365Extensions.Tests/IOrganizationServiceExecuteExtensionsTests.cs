using FakeItEasy;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class IOrganizationServiceExecuteExtensionsTests
    {
        [TestMethod()]
        public void ExecuteTest()
        {
            //Setup
            var context = new XrmFakedContext();
            var service = context.GetOrganizationService();

            var collection = new List<OrganizationRequest>()
            {
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
                new OrganizationRequest(),
            };

            var size = collection.Count / 2 + 1;

            //We dont use Skip+Take in real method as they iterate over collection from the beginning for each attempt
            List<OrganizationRequest> expectedChunk1 = collection.Take(size).ToList();
            List<OrganizationRequest> expectedChunk2 = collection.Skip(size).Take(size).ToList();

            var settings = new ExecuteMultipleSettings()
            {
                ContinueOnError = true,
                ReturnResponses = true,
            };

            var expectedResponses = new List<ExecuteMultipleResponse>()
            {
                new ExecuteMultipleResponse(),
                new ExecuteMultipleResponse(),
                new ExecuteMultipleResponse(),
            };

            var fakeResponceEnumerator = expectedResponses.GetEnumerator();

            List<ExecuteMultipleRequest> actualRequests = new List<ExecuteMultipleRequest>();

            context.AddExecutionMock<ExecuteMultipleRequest>((request) =>
            {
                actualRequests.Add(request as ExecuteMultipleRequest);

                fakeResponceEnumerator.MoveNext();
                return fakeResponceEnumerator.Current;
            });

            //Act
            var result = service.Execute(collection, size, settings).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);
            var actualResponse1 = result[0];
            var actualResponse2 = result[1];

            //We return expected responses
            Assert.AreEqual(expectedResponses[0], actualResponse1);
            Assert.AreEqual(expectedResponses[1], actualResponse2);

            //Requests are made with correct settings
            Assert.AreEqual(settings, actualRequests[0].Settings);
            Assert.AreEqual(settings, actualRequests[1].Settings);

            //Requests are made with expected bathes
            CollectionAssert.AreEqual(expectedChunk1, actualRequests[0].Requests.ToList());
            CollectionAssert.AreEqual(expectedChunk2, actualRequests[1].Requests.ToList());

            //Responses are updated with corresponding request lists
            CollectionAssert.AreEqual(expectedChunk1, actualResponse1.GetRequests().ToList());
            CollectionAssert.AreEqual(expectedChunk2, actualResponse2.GetRequests().ToList());
        }

        [TestMethod()]
        public void ExecuteMultipleResponseItemUninstrumentedTest()
        {
            //Setup
            var oobItem = new ExecuteMultipleResponseItem
            {
                RequestIndex = 5
            };

            var oobResponse = new ExecuteMultipleResponse();

            //Act
            var requests = oobResponse.GetRequests();
            var request = oobResponse.GetRequest(oobItem);

            //Assert
            Assert.IsNull(requests);
            Assert.IsNull(request);
        }

        [TestMethod()]
        public void ExecuteShouldStopAfterFirstErrorTest()
        {
            //Setup
            var context = new XrmFakedContext();
            var service = context.GetOrganizationService();

            var requests = new List<OrganizationRequest>()
            {
                new OrganizationRequest() { RequestId = Guid.NewGuid() },
                new OrganizationRequest() { RequestId = Guid.NewGuid() },
                new OrganizationRequest() { RequestId = Guid.NewGuid(), RequestName = "IShouldFail" },
                new OrganizationRequest() { RequestId = Guid.NewGuid() },
                new OrganizationRequest() { RequestId = Guid.NewGuid() },
                new OrganizationRequest() { RequestId = Guid.NewGuid() },
                new OrganizationRequest() { RequestId = Guid.NewGuid() },
            };

            var settings = new ExecuteMultipleSettings()
            {
                ContinueOnError = false
            };

            context.AddExecutionMock<ExecuteMultipleRequest>((request) =>
            {
                var eMultipleRequest = request as ExecuteMultipleRequest;

                var responce = new ExecuteMultipleResponse()
                {
                    ["Responses"] = new ExecuteMultipleResponseItemCollection()
                };

                for (var i = 0; i < eMultipleRequest.Requests.Count; i++)
                {
                    var eMultipleResponceItem = new ExecuteMultipleResponseItem
                    {
                        RequestIndex = i
                    };

                    var orgRequest = eMultipleRequest.Requests[i];

                    if (orgRequest.RequestName != "IShouldFail")
                    {
                        eMultipleResponceItem.Response = new OrganizationResponse()
                        {
                            //to simplify request-responce mathing in a test
                            ResponseName = orgRequest.RequestId.ToString()
                        };
                    }
                    else
                    {
                        eMultipleResponceItem.Fault = new OrganizationServiceFault()
                        {
                            //to simplify request-responce mathing in a test
                            Message = orgRequest.RequestId.ToString()
                        };
                        responce["IsFaulted"] = true;
                    }

                    responce.Responses.Add(eMultipleResponceItem);
                }

                return responce;
            });

            //Act
            var result = service.Execute(requests, 2, settings).ToList();

            //Assert
            //We expect to stop after 2 batches
            Assert.AreEqual(2, result.Count);

            var firstBatch = result[0];
            Assert.AreEqual(firstBatch.Responses.Count, 2);
            Assert.AreEqual(firstBatch.Responses[0].Response.ResponseName, requests[0].RequestId.ToString());
            Assert.AreEqual(firstBatch.Responses[1].Response.ResponseName, requests[1].RequestId.ToString());

            var secondBatch = result[1];
            Assert.AreEqual(secondBatch.Responses.Count, 2);
            Assert.AreEqual(secondBatch.Responses[0].Fault.Message, requests[2].RequestId.ToString());
            Assert.AreEqual(secondBatch.Responses[1].Response.ResponseName, requests[3].RequestId.ToString());

        }
    }
}
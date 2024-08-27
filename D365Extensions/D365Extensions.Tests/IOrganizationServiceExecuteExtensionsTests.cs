#pragma warning disable CS0618 // Type or member is obsolete
using FakeItEasy;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class IOrganizationServiceExecuteExtensionsTests
    {
        [TestMethod()]
        public void ExecuteMultipleRequests_Legacy_Test()
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
        public void ExecuteMultipleRequest_Legacy_ShouldStopAfterFirstErrorTest()
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

        [TestMethod()]
        public void ExecuteMultipleRequestsShouldReturnAllResultsTest()
        {
            //Setup
            var context = new XrmFakedContext();
            var service = context.GetOrganizationService();

            var requests = new List<OrganizationRequest>()
            {
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.FailRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
            };

            int expectedResultsCount = requests.Count;

            var failedOne = requests.Where(r => r.RequestName != null).Single();
            var failedIndex = requests.IndexOf(failedOne);

            //5 response items: 1 have Fault set to a value
            //https://learn.microsoft.com/en-us/power-apps/developer/data-platform/org-service/execute-multiple-requests
            var settings = new ExecuteMultipleSettings()
            {
                ContinueOnError = true,
                ReturnResponses = true,
            };

            int batchSize = 2;

            int expectedEMultipleRequestsCount = (int)Math.Round(expectedResultsCount / (double)batchSize, MidpointRounding.AwayFromZero);

            var expectedRequestsChunks = new List<List<OrganizationRequest>>();

            for (int i = 0; i < expectedEMultipleRequestsCount; i++)
            {
                var expectedRequestsChunk = requests.Skip(i * batchSize).Take(batchSize).ToList();
                expectedRequestsChunks.Add(expectedRequestsChunk);
            }

            //Act
            var actualCallbacks = new List<ExecuteMultipleResponse>();

            Action<OrganizationRequestCollection, ExecuteMultipleResponse> callback = (coll, resp) =>
            {
                actualCallbacks.Add(resp);
            };


            var fakeExecutor = new FakeExecuteMultipleExecutor();
            context.AddFakeMessageExecutor<ExecuteMultipleRequest>(fakeExecutor);

            var result = service.Execute(requests, batchSize, settings, callback).ToList();

            //Assert
            Assert.AreEqual(expectedResultsCount, result.Count);
            Assert.AreEqual(expectedEMultipleRequestsCount, fakeExecutor.ActualRequests.Count);
            Assert.AreEqual(expectedEMultipleRequestsCount, fakeExecutor.Responses.Count);
            Assert.AreEqual(expectedEMultipleRequestsCount, actualCallbacks.Count);

            for (int i = 0; i < expectedEMultipleRequestsCount; i++)
            {
                //setings are set correctly
                ExecuteMultipleRequest actualEMultipleRequest = fakeExecutor.ActualRequests[i];
                Assert.AreEqual(settings, actualEMultipleRequest.Settings);

                //OrgRequests are chunked as expected
                var expectedRequestsChunk = requests.Skip(i * batchSize).Take(batchSize).ToArray();
                CollectionAssert.AreEqual(expectedRequestsChunk, actualEMultipleRequest.Requests);

                //ExecMultiple requests and responses are associated correctly
                //var responseRequests = fakeExecutor.Responses[i].GetRequests();
                //Assert.AreEqual(actualEMultipleRequest.Requests, responseRequests);

                //Callback works as expected
                Assert.AreEqual(fakeExecutor.Responses[i], actualCallbacks[i]);
            }

            for (int i = 0; i < expectedResultsCount; i++)
            {
                //results are associated with correct request
                Assert.AreEqual(requests[i], result[i].Request);
                Assert.AreEqual(requests[i].RequestId, FakeExecuteMultipleExecutor.GetRequestId(result[i].ExecuteMultipleResponseItem));

                if (i != failedIndex)
                {
                    Assert.IsNotNull(result[i].Response);
                    Assert.IsNull(result[i].Fault);
                }
                else
                {
                    Assert.IsNull(result[i].Response);
                    Assert.IsNotNull(result[i].Fault);
                }
            }
        }

        [TestMethod()]
        public void ExecuteMultipleRequestsShouldStopAfterFirstErrorTest()
        {
            //Setup
            var context = new XrmFakedContext();
            var service = context.GetOrganizationService();

            var requests = new List<OrganizationRequest>()
            {
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.FailRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
            };

            //3 response items: 1 have Fault set to a value
            //https://learn.microsoft.com/en-us/power-apps/developer/data-platform/org-service/execute-multiple-requests
            var settings = new ExecuteMultipleSettings()
            {
                ContinueOnError = false,
                ReturnResponses = true,
            };

            int batchSize = 2;

            //Act
            var actualEMultipleResponses = new List<ExecuteMultipleResponse>();

            var callback = new Action<OrganizationRequestCollection, ExecuteMultipleResponse>((coll, resp) =>
            {
                actualEMultipleResponses.Add(resp);
            });


            var fakeExecutor = new FakeExecuteMultipleExecutor();
            context.AddFakeMessageExecutor<ExecuteMultipleRequest>(fakeExecutor);

            var result = service.Execute(requests, batchSize, settings, callback).ToList();

            //Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(2, fakeExecutor.ActualRequests.Count);

            //We have 3 results, one have Fault
            ExecuteMultipleOperationResponse result0 = result[0];
            Assert.IsNotNull(result0.Response);
            Assert.IsNull(result0.Fault);
            Assert.AreEqual(requests[0], result0.Request);

            ExecuteMultipleOperationResponse result1 = result[1];
            Assert.IsNotNull(result1.Response);
            Assert.IsNull(result1.Fault);
            Assert.AreEqual(requests[1], result1.Request);

            ExecuteMultipleOperationResponse result2 = result[2];
            Assert.IsNull(result2.Response);
            Assert.IsNotNull(result2.Fault);
            Assert.AreEqual(requests[2], result2.Request);

            //Callback works as expected
            CollectionAssert.AreEqual(fakeExecutor.Responses, actualEMultipleResponses);
        }
    }
}
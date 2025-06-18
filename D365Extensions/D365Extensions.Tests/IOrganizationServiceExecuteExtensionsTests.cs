using D365Extensions.Tests.Entities;
using FakeItEasy;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
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
                //settings are set correctly
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

        [TestMethod()]
        public void ExecuteMultipleWithQueryTest()
        {
            //Setup
            var context = new XrmFakedContext();
            context.MaxRetrieveCount = 10;

            var entities = new Entity[context.MaxRetrieveCount * 2];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = new Entity()
                {
                    Id = Guid.NewGuid(),
                    LogicalName = "contact"
                };
            }

            context.Initialize(entities);

            var query = new QueryExpression("contact");

            Func<Entity, OrganizationRequest> toReq = (entity) => new UpdateRequest()
            {
                Target = new Entity(entity.LogicalName, entity.Id)
                {
                    ["name"] = "Artem Grunin"
                }
            };

            var settings = new ExecuteMultipleSettings()
            {
                ContinueOnError = true,
                ReturnResponses = true,
            };

            int batchSize = context.MaxRetrieveCount;

            int actualResponseCount = 0;

            Action<ExecuteMultipleOperationResponse> onResponse = (r) => actualResponseCount++;

            var progressReports = new List<ExecuteMultipleProgress>();

            Action<ExecuteMultipleProgress> progress = progressReports.Add;

            IOrganizationService service = context.GetOrganizationService();

            //Act
            service.Execute(query, toReq, settings, batchSize, onResponse, progress);

            //Assert
            var assertQuery = new QueryExpression("contact")
            {
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression("name", ConditionOperator.NotNull)
                    }
                }
            };

            var updatedRecords = service.RetrieveMultiple(assertQuery, null).ToArray();

            Assert.AreEqual(entities.Length, updatedRecords.Length);
            Assert.AreEqual(entities.Length, actualResponseCount);

            //2 page loads, 2 batches and 1 final
            Assert.AreEqual(5, progressReports.Count);

            //10 queried 0 processed
            Assert.AreEqual<uint>(10, progressReports[0].Queried);
            Assert.AreEqual<uint>(0, progressReports[0].Processed);
            Assert.AreEqual<uint>(0, progressReports[0].Errors);
            Assert.AreEqual<uint>(0, progressReports[0].Skipped);

            Assert.AreEqual<float>(0.0F, progressReports[0].Progress);
            Assert.AreEqual<float>(0.0F, progressReports[0].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[0].SkippedRate);

            //10 loaded 10 processed
            Assert.AreEqual<uint>(10, progressReports[1].Queried);
            Assert.AreEqual<uint>(10, progressReports[1].Processed);
            Assert.AreEqual<uint>(0, progressReports[1].Errors);
            Assert.AreEqual<uint>(0, progressReports[1].Skipped);

            Assert.AreEqual<float>(100.0F, progressReports[1].Progress);
            Assert.AreEqual<float>(0.0F, progressReports[1].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[1].SkippedRate);

            //20 loaded 10 processed
            Assert.AreEqual<uint>(20, progressReports[2].Queried);
            Assert.AreEqual<uint>(10, progressReports[2].Processed);
            Assert.AreEqual<uint>(0, progressReports[2].Errors);
            Assert.AreEqual<uint>(0, progressReports[2].Skipped);

            Assert.AreEqual<float>(50.0F, progressReports[2].Progress);
            Assert.AreEqual<float>(0.0F, progressReports[2].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[2].SkippedRate);

            //20 loaded 20 processed
            Assert.AreEqual<uint>(20, progressReports[3].Queried);
            Assert.AreEqual<uint>(20, progressReports[3].Processed);
            Assert.AreEqual<uint>(0, progressReports[3].Errors);
            Assert.AreEqual<uint>(0, progressReports[3].Skipped);

            Assert.AreEqual<float>(100.0F, progressReports[3].Progress);
            Assert.AreEqual<float>(0.0F, progressReports[3].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[3].SkippedRate);

            //final recalculation
            Assert.AreEqual<uint>(20, progressReports[4].Queried);
            Assert.AreEqual<uint>(20, progressReports[4].Processed);
            Assert.AreEqual<uint>(0, progressReports[4].Errors);
            Assert.AreEqual<uint>(0, progressReports[4].Skipped);

            Assert.AreEqual<float>(100.0F, progressReports[4].Progress);
            Assert.AreEqual<float>(0.0F, progressReports[4].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[4].SkippedRate);
        }

        [TestMethod()]
        public void ExecuteMultipleWithQueryMinimalTest()
        {
            //Setup
            var context = new XrmFakedContext();
            context.MaxRetrieveCount = 10;

            var entities = new Entity[context.MaxRetrieveCount + 1];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = new Entity()
                {
                    Id = Guid.NewGuid(),
                    LogicalName = "contact"
                };
            }

            context.Initialize(entities);

            IOrganizationService service = context.GetOrganizationService();

            Func<Entity, OrganizationRequest> toReq = (entity) => new UpdateRequest()
            {
                Target = new Entity(entity.LogicalName, entity.Id)
                {
                    ["name"] = "Artem Grunin"
                }
            };

            var batchQuery = new QueryExpression("contact");

            //Act
            service.Execute(batchQuery, toReq);

            //Assert
            var assertQuery = new QueryExpression("contact")
            {
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression("name", ConditionOperator.NotNull)
                    }
                }
            };

            var updatedRecords = service.RetrieveMultiple(assertQuery, null).ToArray();

            Assert.AreEqual(entities.Length, updatedRecords.Length);
        }

        [TestMethod()]
        public void ExecuteMultipleWithQueryWithNoResultsTest()
        {
            //Setup
            var context = new XrmFakedContext();

            var batchQuery = new QueryExpression("contact");

            Func<Entity, OrganizationRequest> toReq = (entity) =>
            {
                return new UpdateRequest()
                {
                    Target = new Entity(entity.LogicalName, entity.Id)
                    {
                        ["name"] = "Artem Grunin"
                    }
                };
            };

            var progressReports = new List<ExecuteMultipleProgress>();

            Action<ExecuteMultipleProgress> progress = progressReports.Add;

            IOrganizationService service = context.GetOrganizationService();

            //Act
            service.Execute(batchQuery, toReq, onProgress: progress);

            //Assert

            //1 page load and 1 final report
            Assert.AreEqual(2, progressReports.Count);

            //0 loaded mean 100% compleate
            Assert.AreEqual<uint>(0, progressReports[0].Queried);
            Assert.AreEqual<uint>(0, progressReports[0].Processed);
            Assert.AreEqual<uint>(0, progressReports[0].Errors);
            Assert.AreEqual<uint>(0, progressReports[0].Skipped);

            Assert.AreEqual<float>(100.0F, progressReports[0].Progress);
            Assert.AreEqual<float>(0.0F, progressReports[0].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[0].SkippedRate);

            //final recalculation
            Assert.AreEqual<uint>(0, progressReports[1].Queried);
            Assert.AreEqual<uint>(0, progressReports[1].Processed);
            Assert.AreEqual<uint>(0, progressReports[1].Errors);
            Assert.AreEqual<uint>(0, progressReports[1].Skipped);

            Assert.AreEqual<float>(100.0F, progressReports[1].Progress);
            Assert.AreEqual<float>(0.0F, progressReports[1].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[1].SkippedRate);
        }

        [TestMethod()]
        public void ExecuteMultipleWithQueryWithNoRequestsTest()
        {
            //Setup
            var context = new XrmFakedContext();
            context.MaxRetrieveCount = 10;

            var entities = new Entity[context.MaxRetrieveCount * 2];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = new Entity()
                {
                    Id = Guid.NewGuid(),
                    LogicalName = "contact"
                };
            }

            context.Initialize(entities);

            var batchQuery = new QueryExpression("contact");

            //skip this entity
            Func<Entity, OrganizationRequest> toReq = (entity) => null;

            int batchSize = context.MaxRetrieveCount;

            var progressReports = new List<ExecuteMultipleProgress>();

            Action<ExecuteMultipleProgress> progress = (p) =>
            {
                progressReports.Add(p);
            };

            IOrganizationService service = context.GetOrganizationService();

            //Act
            service.Execute(batchQuery, toReq, batchSize: batchSize, onProgress: progress);

            //Assert
            var assertQuery = new QueryExpression("contact")
            {
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression("name", ConditionOperator.NotNull)
                    }
                }
            };

            var updatedRecords = service.RetrieveMultiple(assertQuery, null).ToArray();

            Assert.AreEqual(0, updatedRecords.Length);

            //2 page loads, 0 batches and 1 final recalculation
            Assert.AreEqual(3, progressReports.Count);

            //10 loaded 0 processed
            Assert.AreEqual<uint>(10, progressReports[0].Queried);
            Assert.AreEqual<uint>(0, progressReports[0].Processed);
            Assert.AreEqual<uint>(0, progressReports[0].Errors);
            Assert.AreEqual<uint>(0, progressReports[0].Skipped);

            Assert.AreEqual<float>(0.0F, progressReports[0].Progress);
            Assert.AreEqual<float>(0.0F, progressReports[0].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[0].SkippedRate);

            //20 loaded 0 processed 10 skipped
            Assert.AreEqual<uint>(20, progressReports[1].Queried);
            Assert.AreEqual<uint>(0, progressReports[1].Processed);
            Assert.AreEqual<uint>(0, progressReports[1].Errors);
            Assert.AreEqual<uint>(10, progressReports[1].Skipped);

            Assert.AreEqual<float>(50.0F, progressReports[1].Progress);
            Assert.AreEqual<float>(0.0F, progressReports[1].ErrorRate);
            Assert.AreEqual<float>(50.0F, progressReports[1].SkippedRate);

            //final recalculation
            Assert.AreEqual<uint>(20, progressReports[2].Queried);
            Assert.AreEqual<uint>(0, progressReports[2].Processed);
            Assert.AreEqual<uint>(0, progressReports[2].Errors);
            Assert.AreEqual<uint>(20, progressReports[2].Skipped);

            Assert.AreEqual<float>(100.0F, progressReports[2].Progress);
            Assert.AreEqual<float>(0.0F, progressReports[2].ErrorRate);
            Assert.AreEqual<float>(100.0F, progressReports[2].SkippedRate);
        }

        [TestMethod()]
        public void ExecuteMultipleWithQueryWithErrorsTest()
        {
            //Setup
            var context = new XrmFakedContext();
            context.MaxRetrieveCount = 10;

            var entities = new Entity[context.MaxRetrieveCount * 2];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = new Entity()
                {
                    Id = Guid.NewGuid(),
                    LogicalName = "contact"
                };
            }

            context.Initialize(entities);

            var fakeExecutor = new FakeExecuteMultipleExecutor();
            context.AddFakeMessageExecutor<ExecuteMultipleRequest>(fakeExecutor);

            var query = new QueryExpression("contact");

            Func<Entity, OrganizationRequest> toReq = (entity) =>
            {
                return FakeExecuteMultipleExecutor.FailRequest;
            };

            var settings = new ExecuteMultipleSettings()
            {
                ContinueOnError = true,
                ReturnResponses = false,
            };

            int batchSize = context.MaxRetrieveCount;

            int actualResponceCount = 0;

            Action<ExecuteMultipleOperationResponse> onResponse = (r) =>
            {
                actualResponceCount++;
            };

            var progressReports = new List<ExecuteMultipleProgress>();

            Action<ExecuteMultipleProgress> progress = progressReports.Add;

            IOrganizationService service = context.GetOrganizationService();

            //Act
            service.Execute(query, toReq, settings, batchSize, onResponse, progress);

            //Assert
            var assertQuery = new QueryExpression("contact")
            {
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression("name", ConditionOperator.NotNull)
                    }
                }
            };

            var updatedRecords = service.RetrieveMultiple(assertQuery, null).ToArray();

            Assert.AreEqual(0, updatedRecords.Length);
            Assert.AreEqual(entities.Length, actualResponceCount);

            //2 page loads, 2 batches and 1 final
            Assert.AreEqual(5, progressReports.Count);

            //10 loaded 0 processed 0 errors
            Assert.AreEqual<uint>(10, progressReports[0].Queried);
            Assert.AreEqual<uint>(0, progressReports[0].Processed);
            Assert.AreEqual<uint>(0, progressReports[0].Errors);
            Assert.AreEqual<uint>(0, progressReports[0].Skipped);

            Assert.AreEqual<float>(0.0F, progressReports[0].Progress);
            Assert.AreEqual<float>(0.0F, progressReports[0].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[0].SkippedRate);

            //10 loaded 10 processed 10 errors
            Assert.AreEqual<uint>(10, progressReports[1].Queried);
            Assert.AreEqual<uint>(10, progressReports[1].Processed);
            Assert.AreEqual<uint>(10, progressReports[1].Errors);
            Assert.AreEqual<uint>(0, progressReports[1].Skipped);

            Assert.AreEqual<float>(100.0F, progressReports[1].Progress);
            Assert.AreEqual<float>(100.0F, progressReports[1].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[1].SkippedRate);

            //20 loaded 10 processed 10 errors
            Assert.AreEqual<uint>(20, progressReports[2].Queried);
            Assert.AreEqual<uint>(10, progressReports[2].Processed);
            Assert.AreEqual<uint>(10, progressReports[2].Errors);
            Assert.AreEqual<uint>(0, progressReports[2].Skipped);

            Assert.AreEqual<float>(50.0F, progressReports[2].Progress);
            Assert.AreEqual<float>(100.0F, progressReports[2].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[2].SkippedRate);

            //20 loaded 20 processed 20 errors
            Assert.AreEqual<uint>(20, progressReports[3].Queried);
            Assert.AreEqual<uint>(20, progressReports[3].Processed);
            Assert.AreEqual<uint>(20, progressReports[3].Errors);
            Assert.AreEqual<uint>(0, progressReports[3].Skipped);

            Assert.AreEqual<float>(100.0F, progressReports[3].Progress);
            Assert.AreEqual<float>(100.0F, progressReports[3].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[3].SkippedRate);

            //final recalculation
            Assert.AreEqual<uint>(20, progressReports[4].Queried);
            Assert.AreEqual<uint>(20, progressReports[4].Processed);
            Assert.AreEqual<uint>(20, progressReports[4].Errors);
            Assert.AreEqual<uint>(0, progressReports[4].Skipped);

            Assert.AreEqual<float>(100.0F, progressReports[4].Progress);
            Assert.AreEqual<float>(100.0F, progressReports[4].ErrorRate);
            Assert.AreEqual<float>(0.0F, progressReports[4].SkippedRate);
        }

        [TestMethod()]
        public void UpdateChangedTest()
        {
            // Setup
            const string expectedName = "FixRM";
            const string expectedNumber = "123";

            var existingEntity = new Account
            {
                Id = Guid.NewGuid(),
                Name = "FixRM Corp",
                //AccountNumber = null,
                TickerSymbol = "FM",
            };

            var entity = new Account
            {
                Id = existingEntity.Id,
                // Should update
                Name = expectedName,
                // Should add
                AccountNumber = expectedNumber,
                // Should delete
                TickerSymbol = null,
            };

            var context = new XrmFakedContext();
            context.Initialize(existingEntity);

            Entity actualEntity = null;
            var service = context.GetOrganizationService();
            A.CallTo(() => service.Update(A<Entity>.Ignored)).Invokes((Entity e) => actualEntity = e);

            // Act
            service.UpdateChanged(entity.ToEntity<Entity>());
            var actualAccount = actualEntity.ToEntity<Account>();

            // Assert
            Assert.AreEqual(expectedName, actualAccount.Name);
            Assert.AreEqual(expectedNumber, actualAccount.AccountNumber);
            Assert.IsNull(actualAccount.TickerSymbol);
        }
    }
}
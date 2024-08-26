#pragma warning disable CS0618 // Type or member is obsolete

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
    [TestClass]
    public class FakeExecuteMultipleExecutorTests
    {
        //https://learn.microsoft.com/en-us/power-apps/developer/data-platform/org-service/execute-multiple-requests
        [DataTestMethod]
        [DataRow(true, true, 6, 2, DisplayName = "ContinueOnError=true, ReturnResponses=true 6 response items: 2 have Fault set to a value.")]
        [DataRow(false, true, 3, 1, DisplayName = "ContinueOnError=false, ReturnResponses=true 3 response items: 1 has Fault set to a value.")]
        [DataRow(true, false, 2, 2, DisplayName = "ContinueOnError=true, ReturnResponses=false 2 response items: 2 have Fault set to a value.")]
        [DataRow(false, false, 1, 1, DisplayName = "ContinueOnError=false, ReturnResponses=false 1 response item: 1 has Fault set to a value.")]
        public void ExecuteTest(bool continueOnError, bool returnResponses, int expectedResponsesCount, int expectedErrorCount)
        {
            //Setup
            var requests = new OrganizationRequestCollection()
            {
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.FailRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.FailRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
            };

            var settings = new ExecuteMultipleSettings()
            {
                ContinueOnError = continueOnError,
                ReturnResponses = returnResponses,
            };

            var eMultipleRequest = new ExecuteMultipleRequest()
            {
                Settings = settings,
                Requests = requests
            };

            //Act
            var executor = new FakeExecuteMultipleExecutor();
            var responce = executor.Execute(eMultipleRequest, null) as ExecuteMultipleResponse;

            //Assert
            Assert.AreEqual(expectedResponsesCount, responce.Responses.Count);
            Assert.AreEqual(expectedErrorCount, responce.GetFaultedResponses().Count);
            Assert.IsTrue(responce.IsFaulted);
        }

        [TestMethod]
        public void MiscTest()
        {
            //Setup
            var requests = new OrganizationRequestCollection()
            {
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
            };

            var settings = new ExecuteMultipleSettings()
            {
                ContinueOnError = true,
                ReturnResponses = true,
            };

            var eMultipleRequest = new ExecuteMultipleRequest()
            {
                Settings = settings,
                Requests = requests
            };

            //Act
            var executor = new FakeExecuteMultipleExecutor();
            var responce = executor.Execute(eMultipleRequest, null) as ExecuteMultipleResponse;

            //Assert
            Assert.AreEqual(requests.Count, responce.Responses.Count);
            Assert.AreEqual(0, responce.GetFaultedResponses().Count);

            //Not faulted test
            Assert.IsFalse(responce.IsFaulted);

            //Actual request tracking test
            Assert.AreEqual(1, executor.ActualRequests.Count);
            Assert.AreEqual(eMultipleRequest, executor.ActualRequests[0]);

            //Response traking test
            Assert.AreEqual(1, executor.Responses.Count);
            Assert.AreEqual(responce, executor.Responses[0]);

            for (int i = 0; i < requests.Count; i++)
            {
                //Correct request index test
                Assert.AreEqual(i, responce.Responses[i].RequestIndex);

                //Good requests are coverted as expected test
                Assert.AreEqual(requests[i].RequestId, FakeExecuteMultipleExecutor.GetRequestId(responce.Responses[i]));
            }
        }

        [TestMethod]
        public void FakeXrmEasyIntegrationTest()
        {
            //Setup
            var requests = new OrganizationRequestCollection()
            {
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.FailRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
                FakeExecuteMultipleExecutor.GoodRequest,
            };

            var settings = new ExecuteMultipleSettings()
            {
                ContinueOnError = true,
                ReturnResponses = true,
            };

            var eMultipleRequest = new ExecuteMultipleRequest()
            {
                Settings = settings,
                Requests = requests
            };

            var executor = new FakeExecuteMultipleExecutor();

            var context = new XrmFakedContext();
            context.AddFakeMessageExecutor<ExecuteMultipleRequest>(executor);

            //Act
            var service = context.GetOrganizationService();

            var eMultipleResponce = service.Execute(eMultipleRequest) as ExecuteMultipleResponse;

            //Assert
            //Assert.AreEqual(1, executor.ActualRequests.Count);
            //Assert.AreEqual(eMultipleRequest, executor.ActualRequests[0]);

            Assert.AreEqual(requests.Count, eMultipleResponce.Responses.Count);
        }
    }
}
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
    }
}
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

            ExecuteMultipleResponse expectedResponse = new ExecuteMultipleResponse();

            List<ExecuteMultipleRequest> actualRequests = new List<ExecuteMultipleRequest>();

            context.AddExecutionMock<ExecuteMultipleRequest>((request) =>
            {
                actualRequests.Add(request as ExecuteMultipleRequest);
                
                return expectedResponse;
            });

            ExecuteMultipleResponse actualResponse = null;

            Action<ExecuteMultipleResponse> callback = (resp) => { actualResponse = resp; };

            //Act
            var result = service.Execute(collection, size, settings, callback).ToList();

            //Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expectedResponse, result[0]);
            Assert.AreEqual(expectedResponse, result[1]);

            Assert.AreEqual(settings, actualRequests[0].Settings);
            Assert.AreEqual(settings, actualRequests[1].Settings);

            CollectionAssert.AreEqual(expectedChunk1, actualRequests[0].Requests.ToList());
            CollectionAssert.AreEqual(expectedChunk2, actualRequests[1].Requests.ToList());

            Assert.AreEqual(expectedResponse, actualResponse);
        }
    }
}
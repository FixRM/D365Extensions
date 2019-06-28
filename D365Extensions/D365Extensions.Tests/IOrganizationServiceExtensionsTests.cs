using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class IOrganizationServiceExtensionsTests
    {
        [TestMethod()]
        public void RetrieveMultipleTest()
        {
            /// Setup
            int expectedPages = 10;
            int count = 50;
            int expectedItems = expectedPages * count;

            List<Entity> testData = new List<Entity>(expectedItems);

            for (int i = 0; i < expectedItems; i++)
            {
                testData.Add(new Entity("account", Guid.NewGuid()));
            }

            XrmFakedContext context = new XrmFakedContext();
            context.Initialize(testData);

            QueryExpression query = new QueryExpression("account");
            query.PageInfo.Count = count;

            IOrganizationService service = context.GetOrganizationService();

            /// Act
            int actualPages = 0;
            int actualItems = 0;

            IEnumerable<Entity> result = service.RetrieveMultiple(query, (ec) =>
            {
                actualPages++;
            });

            foreach (Entity entity in result)
            {
                actualItems++;
            }

            /// Assert
            Assert.AreEqual(expectedPages, actualPages);
            Assert.AreEqual(expectedItems, actualItems);
        }

        [TestMethod()]
        public void RetrieveMultipleFetchTest()
        {
            /// Setup
            int expectedPages = 10;
            int count = 50;
            int expectedItems = expectedPages * count;

            string fetch = $@"<fetch count='{ count }' no-lock='true'>
                               <entity name='account' >
                                 <attribute name='name' />
                               </entity>
                              </fetch>";

            List<Entity> testData = new List<Entity>(expectedItems);

            for (int i = 0; i < expectedItems; i++)
            {
                testData.Add(new Entity("account", Guid.NewGuid()));
            }

            XrmFakedContext context = new XrmFakedContext();
            context.Initialize(testData);

            IOrganizationService service = context.GetOrganizationService();

            FetchExpression query = new FetchExpression(fetch);

            /// Act
            int actualPages = 0;
            int actualItems = 0;

            IEnumerable<Entity> result = service.RetrieveMultiple(query, (ec) =>
            {
                actualPages++;
            });

            foreach (Entity entity in result)
            {
                actualItems++;
            }

            /// Assert
            Assert.AreEqual(expectedPages, actualPages);
            Assert.AreEqual(expectedItems, actualItems);
        }
    }
}
#pragma warning disable CS0618 // Type or member is obsolete
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class IOrganizationServiceRetrieveMultipleExtensionsTests
    {
        [TestMethod()]
        public void RetrieveMultipleTest()
        {
            /// Setup
            int expectedPages = 10;
            int count = 50;
            int expectedItemsCount = expectedPages * count;

            var expectedData = new List<Entity>(expectedItemsCount);

            for (int i = 0; i < expectedItemsCount; i++)
            {
                expectedData.Add(new Entity("account", Guid.NewGuid()));
            }

            XrmFakedContext context = new XrmFakedContext();

            context.Initialize(expectedData);

            QueryExpression query = new QueryExpression("account");
            query.PageInfo.Count = count;
            
            IOrganizationService service = context.GetOrganizationService();

            /// Act
            int actualPages = 0;
            int actualItems = 0;
            var actualData = new List<Entity>(expectedItemsCount);

            IEnumerable<Entity> result = service.RetrieveMultiple(query, (ec) =>
            {
                actualPages++;
            });

            foreach (Entity entity in result)
            {
                actualItems++;
                actualData.Add(entity);
            }

            /// Assert
            Assert.AreEqual(expectedPages, actualPages);
            Assert.AreEqual(expectedItemsCount, actualItems);
            CollectionAssert.AreEqual(expectedData, actualData, new EntityIdComparer());
        }

        [TestMethod()]
        public void RetrieveMultipleFetchTest()
        {
            /// Setup
            int expectedPages = 10;
            int count = 50;
            int expectedItems = expectedPages * count;

            string fetch = $@"<fetch count='{count}' no-lock='true'>
                               <entity name='account' >
                                 <attribute name='name' />
                               </entity>
                              </fetch>";

            List<Entity> expectedData = new List<Entity>(expectedItems);

            for (int i = 0; i < expectedItems; i++)
            {
                expectedData.Add(new Entity("account", Guid.NewGuid()));
            }

            XrmFakedContext context = new XrmFakedContext();
            context.Initialize(expectedData);

            IOrganizationService service = context.GetOrganizationService();

            FetchExpression query = new FetchExpression(fetch);

            /// Act
            int actualPages = 0;
            int actualItems = 0;
            List<Entity> actualData = new List<Entity>(expectedItems);

            IEnumerable<Entity> result = service.RetrieveMultiple(query, (ec) =>
            {
                actualPages++;
            });

            foreach (Entity entity in result)
            {
                actualItems++;
                actualData.Add(entity);
            }

            /// Assert
            Assert.AreEqual(expectedPages, actualPages);
            Assert.AreEqual(expectedItems, actualItems);
            CollectionAssert.AreEqual(expectedData, actualData, new EntityIdComparer());
        }
    }
}
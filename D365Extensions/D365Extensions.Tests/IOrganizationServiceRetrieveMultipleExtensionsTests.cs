#pragma warning disable CS0618 // Type or member is obsolete
using D365Extensions.Tests.Entities;
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

        [TestMethod()]
        public void RetrieveMultipleShouldThrowInvalidPageNumberTest()
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

            IOrganizationService service = context.GetOrganizationService();

            QueryExpression query = new QueryExpression("account");
            query.PageInfo.Count = count;
            query.PageInfo.PageNumber = 1;

            /// Act
            int actualPages = 0;
            int actualItems = 0;

            var error = Assert.ThrowsException<ArgumentException>(() =>
            {
                IEnumerable<Entity> result = service.RetrieveMultiple(query, (ec) =>
                {
                    actualPages++;
                });

                foreach (Entity entity in result)
                {
                    actualItems++;
                }
            });

            /// Assert
            Assert.AreEqual(CheckParam.InvalidPageNumberMessage, error.Message);
            Assert.AreEqual(0, actualPages);
            Assert.AreEqual(0, actualItems);
        }

        [TestMethod()]
        public void RetrieveMultipleShouldThrowInvalidPageNumberFetchTest()
        {
            /// Setup
            int expectedPages = 10;
            int count = 50;
            int expectedItems = expectedPages * count;

            string fetch = $@"<fetch count='{count}' no-lock='true' page='1'>
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

            var error = Assert.ThrowsException<ArgumentException>(() =>
            {
                IEnumerable<Entity> result = service.RetrieveMultiple(query, (ec) =>
                {
                    actualPages++;
                });

                foreach (Entity entity in result)
                {
                    actualItems++;
                }
            });

            /// Assert
            Assert.AreEqual(CheckParam.InvalidPageNumberMessage, error.Message);
            Assert.AreEqual(0, actualPages);
            Assert.AreEqual(0, actualItems);
        }

        [TestMethod()]
        public void RetrieveSingleTest()
        {
            // Setup
            Entity entity1 = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = "contact"
            };

            var context = new XrmFakedContext();
            context.Initialize(entity1);

            var service = context.GetOrganizationService();

            /// all records
            var query = new QueryExpression(entity1.LogicalName);

            // Act
            Entity result = service.RetrieveSingle(query);

            // Assert
            Assert.AreEqual(entity1.Id, result.Id);
        }

        [TestMethod()]
        public void RetrieveSingleTTest()
        {
            // Setup
            Entity entity = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = "account"
            };

            var context = new XrmFakedContext();
            context.Initialize(entity);

            var service = context.GetOrganizationService();

            /// all records
            var query = new QueryExpression(entity.LogicalName);

            // Act
            var result = service.RetrieveSingle<Account>(query);

            // Assert
            Assert.AreEqual(entity.Id, result.Id);
        }

        [TestMethod()]
        public void RetrieveSingleShouldThrowTest()
        {
            // Setup
            Entity entity1 = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = "contact"
            };

            Entity entity2 = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = "contact"
            };

            var context = new XrmFakedContext();
            context.Initialize([entity1, entity2]);

            var service = context.GetOrganizationService();

            /// all records
            var query = new QueryExpression(entity1.LogicalName);

            // Act
            var error = Assert.ThrowsException<InvalidOperationException>(()=> service.RetrieveSingle(query));

            // Assert
            Assert.AreEqual(-2146233079, error.HResult);
        }

        [TestMethod()]
        public void RetrieveSingleOrDefaultTest()
        {
            // Setup
            Entity entity1 = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = "contact"
            };

            var context = new XrmFakedContext();
            context.Initialize(entity1);

            var service = context.GetOrganizationService();

            /// all records
            var query = new QueryExpression(entity1.LogicalName);

            // Act
            Entity result = service.RetrieveSingleOrDefault(query);

            // Assert
            Assert.AreEqual(entity1.Id, result.Id);
        }

        public void RetrieveSingleOrDefaultTTest()
        {
            // Setup
            Entity entity1 = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = "account"
            };

            var context = new XrmFakedContext();
            context.Initialize(entity1);

            var service = context.GetOrganizationService();

            /// all records
            var query = new QueryExpression(entity1.LogicalName);

            // Act
            var result = service.RetrieveSingleOrDefault<Account>(query);

            // Assert
            Assert.AreEqual(entity1.Id, result.Id);
        }

        [TestMethod()]
        public void RetrieveSingleOrDefaultShouldReturnDefaultTest()
        {
            // Setup
            var context = new XrmFakedContext();

            var service = context.GetOrganizationService();

            /// all records
            var query = new QueryExpression("contact");

            // Act
            Entity result = service.RetrieveSingleOrDefault(query);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void RetrieveSingleOrDefaultShouldNotThrowTest()
        {
            // Setup
            Entity entity1 = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = "contact"
            };

            Entity entity2 = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = "contact"
            };

            var context = new XrmFakedContext();
            context.Initialize([entity1, entity2]);

            var service = context.GetOrganizationService();

            /// all records
            var query = new QueryExpression(entity1.LogicalName);

            // Act
            var result = service.RetrieveSingleOrDefault(query);

            // Assert
            Assert.AreEqual(entity1.Id, result.Id);
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeXrmEasy;
using D365Extensions.Tests.Entities;
using Microsoft.Xrm.Sdk.Query;
using Task = D365Extensions.Tests.Entities.Task;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class RelationshipQueryCollectionExtensionTests
    {
        [TestMethod()]
        public void AddWithNameTest()
        {
            // Setup
            var expectedSchemaName = nameof(Account.Account_Tasks);
            var expectedQuery = new QueryExpression();

            var collection = new RelationshipQueryCollection();

            // Act
            collection.Add(expectedSchemaName, expectedQuery);

            // Assert
            var actualPair = collection.Single();

            Assert.AreEqual(expectedSchemaName, actualPair.Key.SchemaName);
            Assert.AreEqual(expectedQuery, actualPair.Value);
        }

        [TestMethod()]
        public void AddWithNameAndColumnSetTest()
        {
            // Setup
            var expectedSchemaName = nameof(Account.Account_Tasks);
            var expectedEntityName = Task.EntityLogicalName;
            var expectedColumnSet = new ColumnSet();

            var collection = new RelationshipQueryCollection();

            // Act
            collection.Add(relationshipName: expectedSchemaName, 
                entityLogicalName: expectedEntityName, 
                columnSet: expectedColumnSet);

            // Assert
            var actualPair = collection.Single();
            QueryExpression actualQuery = actualPair.Value as QueryExpression;

            Assert.AreEqual(expectedSchemaName, actualPair.Key.SchemaName);
            Assert.AreEqual(expectedEntityName, actualQuery.EntityName);
            Assert.AreEqual(expectedColumnSet, actualQuery.ColumnSet);
        }

        [TestMethod()]
        public void AddWithExpressionTest()
        {
            // Setup
            var expectedSchemaName = nameof(Account.Account_Tasks);
            var expectedQuery = new QueryExpression();

            var collection = new RelationshipQueryCollection();

            // Act
            collection.Add<Account>(a=> a.Account_Tasks, expectedQuery);

            // Assert
            var actualPair = collection.Single();

            Assert.AreEqual(expectedSchemaName, actualPair.Key.SchemaName);
            Assert.AreEqual(expectedQuery, actualPair.Value);
        }

        [TestMethod()]
        public void AddWithExpressionAndColumnnSetTest()
        {
            // Setup
            var expectedSchemaName = nameof(Account.Account_Tasks);
            var expectedEntityName = Task.EntityLogicalName;
            var expectedColumnSet = new ColumnSet();

            var collection = new RelationshipQueryCollection();

            // Act
            collection.Add<Account>(a=> a.Account_Tasks, expectedColumnSet);

            // Assert
            var actualPair = collection.Single();
            QueryExpression actualQuery = actualPair.Value as QueryExpression;

            Assert.AreEqual(expectedSchemaName, actualPair.Key.SchemaName);
            Assert.AreEqual(expectedEntityName, actualQuery.EntityName);
            Assert.AreEqual(expectedColumnSet, actualQuery.ColumnSet);
        }
    }
}
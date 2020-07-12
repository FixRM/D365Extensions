using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class QueryExpressionExtensionsTests
    {
        [TestMethod()]
        public void AddOrderTest()
        {
            // Setup
            QueryExpression query = new QueryExpression();

            var expectedAttributeName = nameof(TestEntity.ReferenceTypeProperty).ToLower();
            var expectedOrder = OrderType.Ascending;

            // Act
            query.AddOrder<TestEntity>(t => t.ReferenceTypeProperty, expectedOrder);

            // Assert
            Assert.AreEqual(1, query.Orders.Count);

            var actualOrder = query.Orders[0];
            Assert.AreEqual(expectedAttributeName, actualOrder.AttributeName);
            Assert.AreEqual(expectedOrder, actualOrder.OrderType);
        }

        [TestMethod()]
        public void AddLink1Test()
        {
            // Setup
            QueryExpression query = new QueryExpression();

            string expectedToEntityName = EntityTo.EnityLogicalName;
            string expectedFromAttributName = nameof(EntityFrom.FromId).ToLower();
            string expectedToAttributName = nameof(EntityTo.ToId).ToLower();

            // Act
            LinkEntity actualLink = query.AddLink<EntityFrom, EntityTo>(EntityTo.EnityLogicalName, f => f.FromId, t => t.ToId);

            // Assert
            Assert.AreEqual(1, query.LinkEntities.Count);
            Assert.AreEqual(query.LinkEntities[0], actualLink);
            Assert.AreEqual(expectedToEntityName, actualLink.LinkToEntityName);
            Assert.AreEqual(expectedFromAttributName, actualLink.LinkFromAttributeName);
            Assert.AreEqual(expectedToAttributName, actualLink.LinkToAttributeName);
        }

        [TestMethod()]
        public void AddLink2Test()
        {
            // Setup
            QueryExpression query = new QueryExpression();

            string expectedToEntityName = EntityTo.EnityLogicalName;
            string expectedFromAttributName = nameof(EntityFrom.FromId).ToLower();
            string expectedToAttributName = nameof(EntityTo.ToId).ToLower();
            JoinOperator expectedOperator = JoinOperator.LeftOuter;

            // Act
            LinkEntity actualLink = query.AddLink<EntityFrom, EntityTo>(EntityTo.EnityLogicalName, f => f.FromId, t => t.ToId, expectedOperator);

            // Assert
            Assert.AreEqual(1, query.LinkEntities.Count);
            Assert.AreEqual(query.LinkEntities[0], actualLink);
            Assert.AreEqual(expectedToEntityName, actualLink.LinkToEntityName);
            Assert.AreEqual(expectedFromAttributName, actualLink.LinkFromAttributeName);
            Assert.AreEqual(expectedToAttributName, actualLink.LinkToAttributeName);
            Assert.AreEqual(expectedOperator, actualLink.JoinOperator);
        }
    }
}
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
    public class QueryByAttributeExtensionsTests
    {
        [TestMethod()]
        public void AddAttributeTest()
        {
            // Setup
            QueryByAttribute query = new QueryByAttribute();

            var expectedAttributeName = nameof(TestEntity.ReferenceTypeProperty).ToLower();
            var expectedAttributeValue = "not used";

            // Act
            query.AddAttribute<TestEntity>(t => t.ReferenceTypeProperty, expectedAttributeValue);

            // Assert
            Assert.AreEqual(1, query.Attributes.Count);

            var actualAttribute = query.Attributes[0];
            Assert.AreEqual(expectedAttributeName, actualAttribute);

            var actualValue = query.Values[0];
            Assert.AreEqual(expectedAttributeValue, actualValue);
        }

        [TestMethod()]
        public void AddOrderTest()
        {
            // Setup
            QueryByAttribute query = new QueryByAttribute();

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
    }
}
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
    public class OrderExpressionTests
    {
        [TestMethod()]
        public void OrderExpressionTest()
        {
            // Setup
            var expectedAttributeName = nameof(TestEntity.ValueTypeProperty).ToLower();
            var expectedOrderType = OrderType.Descending;

            // Act
            OrderExpression order = new OrderExpression<TestEntity>(t => t.ValueTypeProperty, expectedOrderType);

            // Assert
            Assert.AreEqual(expectedAttributeName, order.AttributeName);
            Assert.AreEqual(expectedOrderType, order.OrderType);
        }

        [TestMethod()]
        public void OrderExpression_Default_Test()
        {
            // Act
            OrderExpression order = new OrderExpression<TestEntity>();

            // Assert not throw
            Assert.IsNotNull(order);
        }

        [TestMethod()]
        public void Null_Test()
        {
            //Setup
            OrderExpression<TestEntity> orderT = null;
            // Act
            OrderExpression order = orderT;

            // Assert not throw
            Assert.IsNull(order);
        }
    }
}
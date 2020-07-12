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
    public class FilterExpressionExtensionsTests
    {
        [TestMethod()]
        public void AddCondition1Test()
        {
            // Setup
            FilterExpression filter = new FilterExpression();

            var expectedAttribute = nameof(TestEntity.ReferenceTypeProperty).ToLower();
            var expectedOperator = ConditionOperator.Equal;
            var expectedValue = "not used";

            // Act
            filter.AddCondition<TestEntity>(t => t.ReferenceTypeProperty, expectedOperator, expectedValue);

            // Assert
            Assert.AreEqual(1, filter.Conditions.Count);
            ConditionExpression actualtCondition = filter.Conditions[0];

            Assert.AreEqual(expectedAttribute, actualtCondition.AttributeName);
            Assert.AreEqual(expectedOperator, actualtCondition.Operator);
            Assert.AreEqual(expectedValue, actualtCondition.Values[0]);
            Assert.IsNull(actualtCondition.EntityName);
        }

        [TestMethod()]
        public void AddCondition2Test()
        {
            // Setup
            FilterExpression filter = new FilterExpression();

            var expectedEntityName = TestEntity.EntityLogicalName;
            var expectedAttribute = nameof(TestEntity.ReferenceTypeProperty).ToLower();
            var expectedOperator = ConditionOperator.Equal;
            var expectedValue = "not used";

            // Act
            filter.AddCondition<TestEntity>(expectedEntityName, t => t.ReferenceTypeProperty, expectedOperator, expectedValue);

            // Assert
            Assert.AreEqual(1, filter.Conditions.Count);
            ConditionExpression actualtCondition = filter.Conditions[0];

            Assert.AreEqual(expectedAttribute, actualtCondition.AttributeName);
            Assert.AreEqual(expectedOperator, actualtCondition.Operator);
            Assert.AreEqual(expectedValue, actualtCondition.Values[0]);
            Assert.AreEqual(expectedEntityName, actualtCondition.EntityName);
        }
    }
}
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
    public class ConditionExpressionTests
    {
        [TestMethod()]
        public void Constructor_Full_Test()
        {
            // Setup
            var expectedEntityName = TestEntity.EntityLogicalName;
            var expectedAttributeName = nameof(TestEntity.ValueTypeProperty).ToLower();
            var expectedOperator = ConditionOperator.Equal;
            var expectedValue = new object[] { 42 };

            // Act
            ConditionExpression condition = new ConditionExpression<TestEntity>(
                expectedEntityName,
                t => t.ValueTypeProperty,
                expectedOperator,
                expectedValue);

            // Assert
            Assert.AreEqual(expectedEntityName, condition.EntityName);
            Assert.AreEqual(expectedAttributeName, condition.AttributeName);
            Assert.AreEqual(expectedOperator, condition.Operator);
            CollectionAssert.AreEqual(expectedValue, condition.Values.ToArray());
        }

        [TestMethod()]
        public void Constructor_No_Entity_Name_Test()
        {
            // Setup
            var expectedAttributeName = nameof(TestEntity.ValueTypeProperty).ToLower();
            var expectedOperator = ConditionOperator.Equal;
            var expectedValue = new object[] { 42 };

            // Act
            ConditionExpression condition = new ConditionExpression<TestEntity>(
                t => t.ValueTypeProperty,
                expectedOperator,
                expectedValue);

            // Assert
            Assert.IsNull(condition.EntityName);
            Assert.AreEqual(expectedAttributeName, condition.AttributeName);
            Assert.AreEqual(expectedOperator, condition.Operator);
            CollectionAssert.AreEqual(expectedValue, condition.Values.ToArray());
        }

        [TestMethod()]
        public void Constructor_Single_Value_Test()
        {
            // Setup
            var expectedAttributeName = nameof(TestEntity.ValueTypeProperty).ToLower();
            var expectedOperator = ConditionOperator.Equal;
            int expectedValue = 42;

            // Act
            ConditionExpression condition = new ConditionExpression<TestEntity>(
                t => t.ValueTypeProperty,
                expectedOperator,
                expectedValue);

            // Assert
            Assert.IsNull(condition.EntityName);
            Assert.AreEqual(expectedAttributeName, condition.AttributeName);
            Assert.AreEqual(expectedOperator, condition.Operator);
            Assert.AreEqual(expectedValue, condition.Values[0]);
        }

        [TestMethod()]
        public void Constructor_Mimimal_Test()
        {
            // Setup
            var expectedAttributeName = nameof(TestEntity.ValueTypeProperty).ToLower();
            var expectedOperator = ConditionOperator.Null;

            // Act
            ConditionExpression condition = new ConditionExpression<TestEntity>(
                t => t.ValueTypeProperty,
                expectedOperator);

            // Assert
            Assert.IsNull(condition.EntityName);
            Assert.AreEqual(expectedAttributeName, condition.AttributeName);
            Assert.AreEqual(expectedOperator, condition.Operator);
            Assert.AreEqual(0, condition.Values.Count);
        }

        [TestMethod()]
        public void Constructor_Default_Test()
        {
            //Setup
            ConditionExpression conditionDefault = new ConditionExpression();

            // Act
            ConditionExpression condition = new ConditionExpression<TestEntity>();

            // Assert
            Assert.AreEqual(conditionDefault.EntityName, condition.EntityName);
            Assert.AreEqual(conditionDefault.AttributeName, condition.AttributeName);
            Assert.AreEqual(conditionDefault.Operator, condition.Operator);
            Assert.AreEqual(0, condition.Values.Count);
        }

        [TestMethod()]
        public void Null_Test()
        {
            // Setup
            ConditionExpression<TestEntity> conditionT = null;
            
            // Act
            ConditionExpression condition = conditionT;

            // Assert not throw
            Assert.IsNull(condition);
        }
    }
}
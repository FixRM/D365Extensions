using D365Extensions.Tests.Entities;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = D365Extensions.Tests.Entities.Task;

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
            ConditionExpression actualCondition = filter.Conditions[0];

            Assert.AreEqual(expectedAttribute, actualCondition.AttributeName);
            Assert.AreEqual(expectedOperator, actualCondition.Operator);
            Assert.AreEqual(expectedValue, actualCondition.Values[0]);
        }

        [TestMethod()]
        public void AddCondition2Test()
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
            ConditionExpression actualCondition = filter.Conditions[0];

            Assert.AreEqual(expectedAttribute, actualCondition.AttributeName);
            Assert.AreEqual(expectedOperator, actualCondition.Operator);
            Assert.AreEqual(expectedValue, actualCondition.Values[0]);
        }

        [TestMethod()]
        public void QueryTest()
        {
            var account = new Account()
            {
                Id = Guid.NewGuid(),
            };

            var task = new Task()
            {
                Id = Guid.NewGuid(),
                RegardingObjectId = account.ToEntityReference(),
                Description = "Test"
            };

            var query = new QueryExpression(Account.EntityLogicalName);

            var link = query.AddLink<Account, Task>(
                a => a.AccountId,
                p => p.RegardingObjectId,
                JoinOperator.Inner);

            link.LinkCriteria.AddCondition<Task>(p => p.Description, ConditionOperator.Equal, "Test");
            
            var context = new XrmFakedContext();
            context.Initialize([account, task]);

            var service = context.GetOrganizationService();

            var results = service.RetrieveMultiple(query);
        }
    }
}
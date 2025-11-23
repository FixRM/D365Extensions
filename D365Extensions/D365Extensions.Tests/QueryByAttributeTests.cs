using D365Extensions.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class QueryByAttributeTests
    {
        [TestMethod()]
        public void CastTest()
        {
            // Setup
            var query = new QueryByAttribute<Account>()
            {
                TopCount = 1,
                PageInfo = new PagingInfo(),
                ColumnSet = new ColumnSet<Account>(a => a.Name),
                Orders =
                {
                    new OrderExpression<Account>(a=> a.Name, OrderType.Ascending)
                },
                AttributeValues =
                {
                    { a=> a.AccountNumber, "42" },
                    { a=> a.Name, "FixRM" }
                }
            };

            // Act
            QueryByAttribute queryByAttribute = query;

            // Assert
            Assert.AreEqual(Account.EntityLogicalName, queryByAttribute.EntityName);
            Assert.AreEqual(query.TopCount, queryByAttribute.TopCount);
            Assert.AreEqual(query.PageInfo, queryByAttribute.PageInfo);
            CollectionAssert.AreEqual(query.Orders, queryByAttribute.Orders);

            Assert.AreEqual(2, queryByAttribute.Attributes.Count);
            Assert.AreEqual("accountnumber", queryByAttribute.Attributes[0]);
            Assert.AreEqual("name", queryByAttribute.Attributes[1]);
            Assert.AreEqual(2, queryByAttribute.Values.Count);

            Assert.AreEqual("42", queryByAttribute.Values[0]);
            Assert.AreEqual("FixRM", queryByAttribute.Values[1]);
        }

        [TestMethod()]
        public void AddOrderTest()
        {
            //Setup
            var query = new QueryByAttribute<Account>();

            //Act
            query.AddOrder(a => a.Name, OrderType.Descending);

            //Assert
            Assert.AreEqual(1, query.Orders.Count);
            Assert.AreEqual("name", query.Orders[0].AttributeName);
            Assert.AreEqual(OrderType.Descending, query.Orders[0].OrderType);
        }

        [TestMethod()]
        public void AddAttributeValueTest()
        {
            //Setup
            Expression<Func<Account, object>> expectedAttributeName = a => a.Name;
            var expectedValue = "FixRM";

            var query = new QueryByAttribute<Account>();

            //Act
            query.AddAttributeValue(a => a.Name, expectedValue);

            //Assert
            Assert.AreEqual(1, query.AttributeValues.Count);
            Assert.AreEqual(expectedAttributeName.ToString(), query.AttributeValues.Keys.First().ToString());
            Assert.AreEqual(expectedValue, query.AttributeValues.Values.First());
        }

        [TestMethod()]
        public void NullCastTest()
        {
            //Setup
            QueryByAttribute<Account> query = null;

            //Act
            QueryByAttribute queryByAttribute = query;

            //Assert
            Assert.IsNull(queryByAttribute);
        }
    }
}
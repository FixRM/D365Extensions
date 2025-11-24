using D365Extensions.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class QueryExpressionTests
    {
        [TestMethod()]
        public void CastTest()
        {
            //Setup
            var query = new QueryExpression<Account>()
            {
                TopCount = 1,
                PageInfo = new PagingInfo(),
                Criteria = new FilterExpression(),
                Distinct = true,
                ColumnSet = new ColumnSet<Account>(a => a.Name),
                NoLock = true,
                LinkEntities =
                {
                    new LinkEntity<Account, Task>(a=> a.AccountId, t=> t.RegardingObjectId, JoinOperator.Inner)
                },
                Orders =
                {
                    new OrderExpression<Account>(a=> a.CreatedOn, OrderType.Descending)
                }
            };

            //Act
            QueryExpression queryExpression = query;

            //Assert
            Assert.AreEqual(Account.EntityLogicalName, queryExpression.EntityName);
            Assert.AreEqual(query.TopCount, queryExpression.TopCount);
            Assert.AreEqual(query.PageInfo, queryExpression.PageInfo);
            Assert.AreEqual(query.Criteria, queryExpression.Criteria);
            Assert.AreEqual(query.Distinct, queryExpression.Distinct);
            Assert.AreEqual(query.ColumnSet, queryExpression.ColumnSet);
            Assert.AreEqual(query.NoLock, queryExpression.NoLock);
            CollectionAssert.AreEqual(query.LinkEntities, queryExpression.LinkEntities);
            CollectionAssert.AreEqual(query.Orders, queryExpression.Orders);
        }

        [TestMethod()]
        public void NullCastTest()
        {
            //Setup
            QueryExpression<Account> query = null;

            //Act
            QueryExpression queryExpression = query;

            //Assert
            Assert.IsNull(queryExpression);
        }

        [TestMethod()]
        public void AddOrderTest()
        {
            //Setup
            var query = new QueryExpression<Account>();

            //Act
            query.AddOrder(a=> a.Name, OrderType.Descending);

            //Assert
            Assert.AreEqual(1, query.Orders.Count);
            Assert.AreEqual("name", query.Orders[0].AttributeName);
            Assert.AreEqual(OrderType.Descending, query.Orders[0].OrderType);
        }

        [TestMethod()]
        public void AddLinkTest()
        {
            //Setup
            var query = new QueryExpression<Account>();

            //Act
            var link = query.AddLink<Account, Task>(a=> a.AccountId, t=> t.RegardingObjectId, JoinOperator.Inner);

            //Assert
            Assert.AreEqual(1, query.LinkEntities.Count);
            Assert.AreEqual(link, query.LinkEntities[0]);
            Assert.AreEqual(Account.EntityLogicalName, query.LinkEntities[0].LinkFromEntityName);
            Assert.AreEqual("accountid", query.LinkEntities[0].LinkFromAttributeName);
            Assert.AreEqual(Task.EntityLogicalName, query.LinkEntities[0].LinkToEntityName);
            Assert.AreEqual("regardingobjectid", query.LinkEntities[0].LinkToAttributeName);
            Assert.AreEqual(JoinOperator.Inner, query.LinkEntities[0].JoinOperator);
        }
    }
}
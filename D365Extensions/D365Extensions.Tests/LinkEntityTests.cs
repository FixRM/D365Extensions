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
    public class LinkEntityTests
    {
        [TestMethod()]
        public void LinkEntityTest()
        {
            // Setup
            string expectedFromEntityName = EntityFrom.EnityLogicalName;
            string expectedToEntityName = EntityTo.EnityLogicalName;
            string expectedFromAttributName = nameof(EntityFrom.FromId).ToLower();
            string expectedToAttributName = nameof(EntityTo.ToId).ToLower();
            JoinOperator expectedOperator = JoinOperator.LeftOuter;
            string expectedAlias = "Alias";
            ColumnSet expectedColumnSet = new ColumnSet();
            FilterExpression expectedFilter = new FilterExpression();
            OrderExpression expectedOrder = new OrderExpression();
            LinkEntity expectedLink = new LinkEntity();

            // Act
            LinkEntity<EntityFrom, EntityTo> linkGen = new LinkEntity<EntityFrom, EntityTo>(
                f => f.FromId,
                t => t.ToId,
                expectedOperator);
            linkGen.EntityAlias = expectedAlias;
            linkGen.Columns = expectedColumnSet;
            linkGen.LinkCriteria = expectedFilter;
            linkGen.Orders.Add(expectedOrder);
            linkGen.LinkEntities.Add(expectedLink);

            // implicit cast
            LinkEntity linkEntity = linkGen;

            // Assert
            Assert.AreEqual(expectedFromEntityName, linkEntity.LinkFromEntityName);
            Assert.AreEqual(expectedToEntityName, linkEntity.LinkToEntityName);
            Assert.AreEqual(expectedFromAttributName, linkEntity.LinkFromAttributeName);
            Assert.AreEqual(expectedToAttributName, linkEntity.LinkToAttributeName);
            Assert.AreEqual(expectedOperator, linkEntity.JoinOperator);
            Assert.AreEqual(expectedAlias, linkEntity.EntityAlias);
            Assert.AreEqual(expectedColumnSet, linkEntity.Columns);
            Assert.AreEqual(expectedFilter, linkEntity.LinkCriteria);
            Assert.AreEqual(1, linkEntity.Orders.Count);
            Assert.AreEqual(expectedOrder, linkEntity.Orders[0]);
            Assert.AreEqual(1, linkEntity.LinkEntities.Count);
            Assert.AreEqual(expectedLink, linkEntity.LinkEntities[0]);
        }

        [TestMethod()]
        public void LinkEntity_Default_Test()
        {
            // Setup
            JoinOperator expectedOperator = JoinOperator.Inner;

            // Act
            LinkEntity linkEntity = new LinkEntity<EntityFrom, EntityTo>();

            // Assert
            Assert.AreEqual(expectedOperator, linkEntity.JoinOperator);
        }
    }
}
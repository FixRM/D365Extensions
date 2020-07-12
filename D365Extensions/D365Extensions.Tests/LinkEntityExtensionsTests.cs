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
    public class LinkEntityExtensionsTests
    {
        [TestMethod()]
        public void AddLink1Test()
        {
            // Setup
            string expectedFromEntityName = EntityFrom.EnityLogicalName;
            string expectedToEntityName = EntityTo.EnityLogicalName;
            string expectedFromAttributName = nameof(EntityFrom.FromId).ToLower();
            string expectedToAttributName = nameof(EntityTo.ToId).ToLower();
            JoinOperator expectedOperator = JoinOperator.LeftOuter;

            LinkEntity linkEntity = new LinkEntity()
            {
                LinkFromEntityName = expectedFromEntityName
            };

            // Act
            LinkEntity newLink = linkEntity.AddLink<EntityFrom, EntityTo>(
                EntityTo.EnityLogicalName,
                f=> f.FromId,
                t=> t.ToId,
                expectedOperator);

            // Assert
            Assert.AreEqual(expectedFromEntityName, newLink.LinkFromEntityName);
            Assert.AreEqual(expectedToEntityName, newLink.LinkToEntityName);
            Assert.AreEqual(expectedToAttributName, newLink.LinkToAttributeName);
            Assert.AreEqual(expectedFromAttributName, newLink.LinkFromAttributeName);
            Assert.AreEqual(expectedOperator, newLink.JoinOperator);
        }

        [TestMethod()]
        public void AddLink2Test()
        {
            // Setup
            string expectedFromEntityName = EntityFrom.EnityLogicalName;
            string expectedToEntityName = EntityTo.EnityLogicalName;
            string expectedFromAttributName = nameof(EntityFrom.FromId).ToLower();
            string expectedToAttributName = nameof(EntityTo.ToId).ToLower();

            LinkEntity linkEntity = new LinkEntity()
            {
                LinkFromEntityName = expectedFromEntityName
            };

            // Act
            LinkEntity newLink = linkEntity.AddLink<EntityFrom, EntityTo>(
                EntityTo.EnityLogicalName,
                f => f.FromId,
                t => t.ToId);

            // Assert
            Assert.AreEqual(expectedFromEntityName, newLink.LinkFromEntityName);
            Assert.AreEqual(expectedToEntityName, newLink.LinkToEntityName);
            Assert.AreEqual(expectedToAttributName, newLink.LinkToAttributeName);
            Assert.AreEqual(expectedFromAttributName, newLink.LinkFromAttributeName);
        }
    }
}
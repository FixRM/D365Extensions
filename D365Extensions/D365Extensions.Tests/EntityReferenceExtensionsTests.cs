using D365Extensions.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class EntityReferenceExtensionsTests
    {
        [TestMethod()]
        public void ToEntityTest()
        {
            // Setup
            var reference = new EntityReference()
            {
                Id = Guid.NewGuid(),
                LogicalName = "account",
                Name = "FixRM",
                KeyAttributes =
                {
                    { "code", "123" }
                },
                RowVersion = "456"
            };

            // Act
            var entity = reference.ToEntity();

            // Assert
            Assert.AreEqual(reference.Id, entity.Id);
            Assert.AreEqual(reference.LogicalName, entity.LogicalName);
            Assert.AreEqual(reference.KeyAttributes, entity.KeyAttributes);
            Assert.AreEqual(reference.RowVersion, entity.RowVersion);
        }

        [TestMethod()]
        public void ToEntityTTest()
        {
            // Setup
            var reference = new EntityReference()
            {
                Id = Guid.NewGuid(),
                LogicalName = "account",
                Name = "FixRM",
                KeyAttributes =
                {
                    { "code", "123" }
                },
                RowVersion = "456"
            };

            // Act
            Account entity = reference.ToEntity<Account>();

            // Assert
            Assert.AreEqual(reference.Id, entity.Id);
            Assert.AreEqual(reference.LogicalName, entity.LogicalName);
            Assert.AreEqual(reference.KeyAttributes, entity.KeyAttributes);
            Assert.AreEqual(reference.RowVersion, entity.RowVersion);
        }
    }
}
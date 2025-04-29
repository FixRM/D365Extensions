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
    public class EntityReferenceTests
    {
        [TestMethod()]
        public void ConstructorTest()
        {
            // Act
            EntityReference reference = new EntityReference<Account>();

            // Assert
            Assert.AreEqual(Account.EntityLogicalName, reference.LogicalName);
        }

        [TestMethod()]
        public void Constructor2Test()
        {
            // Setup
            var expectedId = Guid.NewGuid();

            // Act
            EntityReference reference = new EntityReference<Account>(expectedId);

            // Assert
            Assert.AreEqual(Account.EntityLogicalName, reference.LogicalName);
            Assert.AreEqual(expectedId, reference.Id);
        }

        [TestMethod()]
        public void Constructor3Test()
        {
            // Setup
            var expectedKeys = new KeyAttributeCollection();

            // Act
            EntityReference reference = new EntityReference<Account>(expectedKeys);

            // Assert
            Assert.AreEqual(Account.EntityLogicalName, reference.LogicalName);
            Assert.AreEqual(expectedKeys, reference.KeyAttributes);
        }

        [TestMethod()]
        public void Constructor4Test()
        {
            // Setup
            var expectedKey = "accountnumber";
            var expectedValue = "123";

            // Act
            EntityReference reference = new EntityReference<Account>(a=> a.AccountNumber, expectedValue);
            var actualKey = reference.KeyAttributes.Single();

            // Assert
            Assert.AreEqual(Account.EntityLogicalName, reference.LogicalName);
            Assert.AreEqual(expectedKey, actualKey.Key);
            Assert.AreEqual(expectedValue, actualKey.Value);
        }

        [TestMethod()]
        public void EqualsTest()
        {
            // Setup
            var id = Guid.NewGuid();
            var rowVersion = "123";
            var name = "FixRM";
            var key = "accountnumber";
            var value = "42";

            var reference1 = new EntityReference()
            {
                Id = id,
                Name = name,
                LogicalName = Account.EntityLogicalName,
                RowVersion = rowVersion,
                KeyAttributes =
                {
                    { key, value }
                }
            };

            var reference2 = new EntityReference<Account>()
            {
                Id = id,
                Name = name,
                RowVersion = rowVersion,
                KeyAttributes =
                {
                    { key, value }
                }
            };

            // Act + Assert
            Assert.IsTrue(reference1.Equals((EntityReference) reference2));
            Assert.IsTrue(reference2.Equals(reference1));
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            // Setup
            var id = Guid.NewGuid();
            var rowVersion = "123";
            var name = "FixRM";
            var key = "accountnumber";
            var value = "42";

            var reference1 = new EntityReference()
            {
                Id = id,
                Name = name,
                LogicalName = Account.EntityLogicalName,
                RowVersion = rowVersion,
                KeyAttributes =
                {
                    { key, value }
                }
            };

            var reference2 = new EntityReference<Account>()
            {
                Id = id,
                Name = name,
                RowVersion = rowVersion,
                KeyAttributes =
                {
                    { key, value }
                }
            };

            // Act
            var code1 = reference1.GetHashCode();
            var code2 = reference2.GetHashCode();

            // Assert
            Assert.AreEqual(code1, code2);
        }
    }
}
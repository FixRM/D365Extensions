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
    public class AliasedValueTests
    {
        [TestMethod()]
        public void AliasedValueTest()
        {
            // Setup
            var expectedEntityName = "account";
            var expectedAttributeName = "accountnumber";
            var expectedValue = "42";

            // Act
            AliasedValue aliasedValue = new AliasedValue<Account>(a=> a.AccountNumber, expectedValue);

            // Assert
            Assert.AreEqual(expectedEntityName, aliasedValue.EntityLogicalName);
            Assert.AreEqual(expectedAttributeName, aliasedValue.AttributeLogicalName);
            Assert.AreEqual(expectedValue, aliasedValue.Value);
        }

        [TestMethod()]
        public void NullTest()
        {
            // Setup
            AliasedValue<Account> aliasedValueT = null;

            // Act
            AliasedValue aliasedValue = aliasedValueT;

            // Assert
            Assert.IsNull(aliasedValue);
        }
    }
}
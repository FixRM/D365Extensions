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
    public class KeyAttributeCollectionExtensionsTests
    {
        [TestMethod()]
        public void AddTest()
        {
            // Setup
            const string expectedKey = "accountnumber";
            const string expectedValue = "42";

            var keyAttributeCollection = new KeyAttributeCollection();

            // Act
            keyAttributeCollection.Add<Account>(a => a.AccountNumber, expectedValue);
            var actualKeyAttributes = keyAttributeCollection.SingleOrDefault();
            
            // Assert
            Assert.AreEqual(expectedKey, actualKeyAttributes.Key);
            Assert.AreEqual(expectedValue, actualKeyAttributes.Value);
        }
    }
}
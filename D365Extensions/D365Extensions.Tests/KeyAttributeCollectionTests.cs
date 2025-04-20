using D365Extensions.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk.Tests
{
    [TestClass()]
    public class KeyAttributeCollectionTests
    {
        [TestMethod()]
        public void KeyAttributeCollectionTest()
        {
            // Setup
            var expectedKey = "name";
            var expectedValue = "FixRM";

            var keys = new KeyAttributeCollection<Account>
            {
                { a => a.Name, expectedValue}
            };

            // Act
            KeyAttributeCollection actualKeys = keys;
            var kv = actualKeys.Single();

            // Assert
            Assert.AreEqual(expectedKey, kv.Key);
            Assert.AreEqual(expectedValue, kv.Value);
        }
    }
}
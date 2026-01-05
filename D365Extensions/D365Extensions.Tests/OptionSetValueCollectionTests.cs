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
    public class OptionSetValueCollectionTests
    {
        [TestMethod()]
        public void OptionSetValueCollectionTest()
        {
            // Setup
            var expectedValue = StateCode.Active;
            var optionSet = new OptionSetValue<StateCode>(expectedValue);

            // Act
            OptionSetValueCollection optionSetValues = new OptionSetValueCollection<StateCode>([optionSet]);

            // Assert
            Assert.AreEqual((int) expectedValue, optionSetValues.Single().Value);
        }

        [TestMethod()]
        public void OptionSetValueCollectionNullTest()
        {
            // Setup
            OptionSetValueCollection<StateCode> optionSetValues = null;

            // Act
            OptionSetValueCollection values = optionSetValues;

            // Assert
            Assert.IsNull(values);
        }
    }
}
using D365Extensions.Tests;
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
    public class OptionSetValueTests
    {
        [TestMethod()]
        public void OptionSetValueTest()
        {
            // Setup
            var expectedValue = StateCode.Active;

            // Act
            OptionSetValue optionSet = new OptionSetValue<StateCode>(expectedValue);

            // Assert
            Assert.AreEqual((int) expectedValue, optionSet.Value);
        }
    }
}
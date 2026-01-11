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

        [TestMethod()]
        public void OptionSetValueNullCastTest()
        {
            // Setup
            OptionSetValue<StateCode> optionSetValueT = null;

            // Act
            OptionSetValue optionSet = optionSetValueT;

            // Assert
            Assert.IsNull(optionSet);
        }

        [TestMethod()]
        public void OptionSetValueNullTest()
        {
            // Setup
            var optionSetT = new OptionSetValue<StateCode>();

            // Act
            OptionSetValue optionSet = optionSetT;

            // Assert
            Assert.AreEqual(default, optionSet.Value);
        }

        [TestMethod()]
        public void EqualsTest()
        {
            //Setup
            var optionSet1 = new OptionSetValue<StateCode>(StateCode.Active);
            var optionSet2 = new OptionSetValue<StateCode>(StateCode.Active);
            var optionSet3 = new OptionSetValue<StateCode>(StateCode.Inactive);

            //Act
            var shouldBeTrue = optionSet1.Equals(optionSet2);
            var shouldBeFalse= optionSet2.Equals(optionSet3);

            //Assert
            Assert.IsTrue(shouldBeTrue);
            Assert.IsFalse(shouldBeFalse);
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            // Setup
            StateCode stateCode = StateCode.Inactive;
            var expactedHashCode = stateCode.GetHashCode();
            var optionSet = new OptionSetValue<StateCode>(stateCode);

            // Act
            var actualHashCode = optionSet.GetHashCode();

            // Assert
            Assert.AreEqual(expactedHashCode, actualHashCode);
        }
    }
}
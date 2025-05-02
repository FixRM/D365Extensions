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
    public class AliasedValueExtensionsTests
    {
        [TestMethod()]
        public void IsPrimaryKeyShouldReturnTrueTest()
        {
            // Setup
            var av = new AliasedValue(
                entityLogicalName: "account",
                attributeLogicalName: "accountid",
                value: Guid.NewGuid());

            // Act
            var idPrimaryKey = av.IsPrimaryKey();

            // Assert
            Assert.IsTrue(idPrimaryKey);
        }

        [TestMethod()]
        public void IsPrimaryKeyShouldReturnTrueForActivityTest()
        {
            FakeXrmEasy.XrmFakedContext context = new FakeXrmEasy.XrmFakedContext();


            // Setup
            var av = new AliasedValue(
                entityLogicalName: "task",
                attributeLogicalName: "activityid",
                value: Guid.NewGuid());

            // Act
            var idPrimaryKey = av.IsPrimaryKey();

            // Assert
            Assert.IsTrue(idPrimaryKey);
        }

        [TestMethod()]
        public void IsPrimaryKeyShouldReturnFalseTest()
        {
            // Setup
            var av = new AliasedValue(
                entityLogicalName: "account",
                attributeLogicalName: "accountnumber",
                value: "42");

            // Act
            var idPrimaryKey = av.IsPrimaryKey();

            // Assert
            Assert.IsFalse(idPrimaryKey);
        }

        [TestMethod()]
        public void GetValueTest()
        {
            // Setup
            var av = new AliasedValue(
                entityLogicalName: "account",
                attributeLogicalName: "accountnumber",
                value: "42");

            // Act
            var actualValue = av.GetValue<string>();

            // Assert
            Assert.AreEqual("42", actualValue);
        }
    }
}
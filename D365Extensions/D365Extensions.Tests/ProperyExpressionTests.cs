using Microsoft.VisualStudio.TestTools.UnitTesting;
using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class ProperyExpressionTests
    {
        [TestMethod()]
        public void Get_Reference_Type_Property_Name_Test()
        {
            // Setup
            string expected = nameof(TestEntity.ReferenceTypeProperty).ToLower();

            // Act
            string actual = ProperyExpression.GetName<TestEntity>(t => t.ReferenceTypeProperty);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Get_Value_Type_Property_Name_Test()
        {
            // Setup
            string expected = nameof(TestEntity.ValueTypeProperty).ToLower();

            // Act
            string actual = ProperyExpression.GetName<TestEntity>(t => t.ValueTypeProperty);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Get_Names_Test()
        {
            // Setup
            string expected1 = nameof(TestEntity.ReferenceTypeProperty).ToLower();
            string expected2 = nameof(TestEntity.ValueTypeProperty).ToLower();

            // Act
            List<string> actual = ProperyExpression.GetNames<TestEntity>(
                t => t.ReferenceTypeProperty,
                t => t.ValueTypeProperty);

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }

        [TestMethod()]
        public void Should_Throw_On_Methods_Test()
        {
            // Assert
            ArgumentException e = Assert.ThrowsException<ArgumentException>(() =>
            {
                // Act
                ProperyExpression.GetName<TestEntity>(t => t.ToEntityReference());
            });

            Assert.IsTrue(e.Message.Contains(CheckParam.InvalidExpression(null).Message));
        }
    }
}
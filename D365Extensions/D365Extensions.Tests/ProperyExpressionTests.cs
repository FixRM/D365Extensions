using Microsoft.VisualStudio.TestTools.UnitTesting;
using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D365Extensions.Tests.Entities;

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

        [TestMethod()]
        public void Get_Name_For_Oob_Generated()
        {
            // Setup
            string expectedString = nameof(Account.AccountNumber).ToLowerInvariant();
            string expectedDouble = nameof(Account.Address1_Longitude).ToLowerInvariant();
            string expectedOptionSet = nameof(Account.AccountRatingCode).ToLowerInvariant();
            string expectedInt = nameof(Account.Address1_UTCOffset).ToLowerInvariant();
            string expectedMoney = nameof(Account.CreditLimit).ToLowerInvariant();
            string expectedBool = nameof(Account.CreditOnHold).ToLowerInvariant();
            string expectedDate = nameof(Account.CreatedOn).ToLowerInvariant();
            string expectedGuid = nameof(Account.AccountId).ToLowerInvariant();
            string expectedReference = nameof(Account.PrimaryContactId).ToLowerInvariant();
            string expectedDecimal = nameof(Account.ExchangeRate).ToLowerInvariant();

            // Act
            List<string> actual = ProperyExpression.GetNames<Account>(
                a => a.AccountNumber,
                a => a.Address1_Longitude,
                a => a.AccountRatingCode,
                a => a.Address1_UTCOffset,
                a => a.CreditLimit,
                a => a.CreditOnHold,
                a => a.CreatedOn,
                a => a.AccountId,
                a => a.PrimaryContactId,
                a => a.ExchangeRate);

            // Assert
            Assert.AreEqual(expectedString, actual[0]);
            Assert.AreEqual(expectedDouble, actual[1]);
            Assert.AreEqual(expectedOptionSet, actual[2]);
            Assert.AreEqual(expectedInt, actual[3]);
            Assert.AreEqual(expectedMoney, actual[4]);
            Assert.AreEqual(expectedBool, actual[5]);
            Assert.AreEqual(expectedDate, actual[6]);
            Assert.AreEqual(expectedGuid, actual[7]);
            Assert.AreEqual(expectedReference, actual[8]);
            Assert.AreEqual(expectedDecimal, actual[9]);
        }

        [TestMethod()]
        public void Same_Prop_Name_Test()
        {
            // Setup
            string expectedPropName = "string_prop";
            string expectedProp2Name = "string_prop2";

            // Act
            string actualPropName = ProperyExpression.GetName<CustomEntity>(c => c.StringProp);
            string actualProp2Name = ProperyExpression.GetName<CustomEntity2>(c => c.StringProp);

            // Assert
            Assert.AreEqual(expectedPropName, actualPropName);
            Assert.AreEqual(expectedProp2Name, actualProp2Name);
        }

        [TestMethod()]
        public void Not_Decorated_Entity_Test()
        {
            // Setup
            string expectedPropName = nameof(NotDecoratedEntity.TheProp).ToLowerInvariant();
 
            // Act
            string actualPropName = ProperyExpression.GetName<NotDecoratedEntity>(e => e.TheProp);

            // Assert
            Assert.AreEqual(expectedPropName, actualPropName);
        }
    }
}
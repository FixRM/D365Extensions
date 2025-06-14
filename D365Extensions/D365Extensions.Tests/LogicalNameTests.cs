using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using D365Extensions.Tests.Entities;
using System.Linq;
using System.Linq.Expressions;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class LogicalNameTests
    {
        [TestMethod()]
        public void Get_Reference_Type_Property_Name_Test()
        {
            // Setup
            string expected = nameof(TestEntity.ReferenceTypeProperty).ToLower();

            // Act
            string actual = LogicalName.GetName<TestEntity>(t => t.ReferenceTypeProperty);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Get_Value_Type_Property_Name_Test()
        {
            // Setup
            string expected = nameof(TestEntity.ValueTypeProperty).ToLower();

            // Act
            string actual = LogicalName.GetName<TestEntity>(t => t.ValueTypeProperty);

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
            List<string> actual = LogicalName.GetNames<TestEntity>([
                t => t.ReferenceTypeProperty,
                t => t.ValueTypeProperty]).ToList();

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
        }

        [TestMethod()]
        public void Should_Throw_On_Methods_Test()
        {
            // Setup
            var expectedMessage = string.Format(CheckParam.InvalidExpressionMessage, "t.ToEntityReference()");

            // Act
            var e = Assert.ThrowsException<ArgumentException>(
                () => LogicalName.GetName<TestEntity>(t => t.ToEntityReference()));

            // Assert
            Assert.IsTrue(e.Message.StartsWith(expectedMessage));
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
            List<string> actual = LogicalName.GetNames<Account>([
                a => a.AccountNumber,
                a => a.Address1_Longitude,
                a => a.AccountRatingCode,
                a => a.Address1_UTCOffset,
                a => a.CreditLimit,
                a => a.CreditOnHold,
                a => a.CreatedOn,
                a => a.AccountId,
                a => a.PrimaryContactId,
                a => a.ExchangeRate]).ToList();

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
            string actualPropName = LogicalName.GetName<CustomEntity>(c => c.StringProp);
            string actualProp2Name = LogicalName.GetName<CustomEntity2>(c => c.StringProp);

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
            string actualPropName = LogicalName.GetName<NotDecoratedEntity>(e => e.TheProp);

            // Assert
            Assert.AreEqual(expectedPropName, actualPropName);
        }

        [TestMethod()]
        public void Parallel_Execution_Test()
        {
            Parallel.For(0, 1000, i => { Get_Name_For_Oob_Generated(); });
        }


        [TestMethod()]
        public void Oob_Entity_Test()
        {
            // Setup
            string expectedName = Account.EntityLogicalName;
            string expectedName2 = CustomEntity.EnityLogicalName;

            // Act
            string actualName = LogicalName.GetName<Account>();
            string actualName2 = LogicalName.GetName<CustomEntity>();

            // Assert
            Assert.AreEqual(expectedName, actualName);
            Assert.AreEqual(expectedName2, actualName2);
        }

        [TestMethod()]
        public void Not_Decorated_Entity_Name_Test()
        {
            // Setup
            string expectedEntityName = nameof(NotDecoratedEntity).ToLowerInvariant();

            // Act
            string actuaEntityName = LogicalName.GetName<NotDecoratedEntity>();

            // Assert
            Assert.AreEqual(expectedEntityName, actuaEntityName);
        }

        [TestMethod()]
        public void Parallel_Execution_EntityName_Test()
        {
            Parallel.For(0, 1000, i => { Oob_Entity_Test(); });
        }

        [TestMethod()]
        public void Id_Test()
        {
            // Act
            var name = LogicalName.GetName<Account>(a => a.Id);

            // Assert
            Assert.AreEqual("accountid", name);
        }

        [TestMethod()]
        public void Reference_Id_Should_Throw_Test()
        {
            // Setup
            var expectedMessage = string.Format(CheckParam.InvalidExpressionMessage, "a.PrimaryContactId.Id");

            // Act
            var error = Assert.ThrowsException<ArgumentException>(() => LogicalName.GetName<Account>(a => a.PrimaryContactId.Id));

            // Assert
            Assert.IsTrue(error.Message.StartsWith(expectedMessage));
        }

        [TestMethod()]
        public void Anonymous_Object_Test()
        {
            // Act
            var names = LogicalName.GetNames<Account>([a => new { a.Id, a.AccountNumber, a.PrimaryContactId }])
                .ToList();

            // Assert
            Assert.AreEqual(3, names.Count);
            Assert.AreEqual("accountid", names[0]);
            Assert.AreEqual("accountnumber", names[1]);
            Assert.AreEqual("primarycontactid", names[2]);
        }

        [TestMethod()]
        public void Anonymous_Object_With_Error_Test()
        {
            // Setup
            var expectedErrorMessage = string.Format(CheckParam.InvalidExpressionMessage, "a.PrimaryContactId.Id");

            Expression<Func<Account, object>> expression = a => new 
            { 
                a.AccountNumber, 
                a.PrimaryContactId.Id 
            };
            
            // Act
            var error = Assert.ThrowsException<ArgumentException>(() => LogicalName.GetNames([expression]).ToList());

            // Assert
            Assert.IsTrue(error.Message.StartsWith(expectedErrorMessage));
        }
    }
}
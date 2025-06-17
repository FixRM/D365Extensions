using D365Extensions.Tests.Entities;
using FakeXrmEasy;
using FakeXrmEasy.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class IOrganizationServiceRetrieveExtensionsTests
    {
        [TestMethod()]
        public void RetrieveTByKeyAndIdPassedAsReferenceTest()
        {
            // Setup
            const string expectedAccountNumber = "42";

            var expectedAccount = new Account()
            {
                Id = Guid.NewGuid(),
                AccountNumber = expectedAccountNumber,
                Name = "FixRM"
            };

            EntityReference reference1 = new EntityReference<Account>(expectedAccount.Id);
            EntityReference reference2 = new EntityReference<Account>(a => a.AccountNumber, expectedAccountNumber);

            var context = new XrmFakedContext();
            context.Initialize(expectedAccount);

            context.InitializeKeyMetadata(entityLogicalName: Account.EntityLogicalName,
                keyAttributeNames: [LogicalName.GetName<Account>(a => a.AccountNumber)]);

            var service = context.GetOrganizationService();

            // Act
            var actualAccount1 = service.Retrieve<Account>(reference1, new ColumnSet(true));
            var actualAccount2 = service.Retrieve<Account>(reference2, new ColumnSet(true));

            // Assert
            Assert.AreEqual(expectedAccount.Id, actualAccount1.Id);
            Assert.AreEqual(expectedAccount.Id, actualAccount2.Id);
        }

        [TestMethod()]
        public void RetrieveTByKeyLambdaTest()
        {
            // Setup
            const string expectedAccountNumber = "42";

            var expectedAccount = new Account()
            {
                Id = Guid.NewGuid(),
                AccountNumber = expectedAccountNumber,
                Name = "FixRM"
            };

            var context = new XrmFakedContext();
            context.Initialize(expectedAccount);

            context.InitializeKeyMetadata(entityLogicalName: Account.EntityLogicalName,
                keyAttributeNames: [LogicalName.GetName<Account>(a => a.AccountNumber)]);

            var service = context.GetOrganizationService();

            // Act
            var actualAccount = service.Retrieve<Account>(
                keyName: k => k.AccountNumber,
                keyValue: expectedAccountNumber,
                columns: c => new { c.AccountNumber, c.Name });

            // Assert
            Assert.AreEqual(expectedAccount.Id, actualAccount.Id);
            Assert.AreEqual(expectedAccount.AccountNumber, actualAccount.AccountNumber);
            Assert.AreEqual(expectedAccount.Name, actualAccount.Name);
        }

        [TestMethod()]
        public void RetrieveTByReferenceTest()
        {
            // Setup
            const string expectedAccountNumber = "42";

            var expectedAccount = new Account()
            {
                Id = Guid.NewGuid(),
                AccountNumber = expectedAccountNumber,
                Name = "FixRM"
            };

            var context = new XrmFakedContext();
            context.Initialize(expectedAccount);

            var service = context.GetOrganizationService();

            // Act
            var actualAccount = service.Retrieve<Account>(
                reference: expectedAccount.ToEntityReference(),
                columns: c => new { c.AccountNumber, c.Name });

            // Assert
            Assert.AreEqual(expectedAccount.Id, actualAccount.Id);
            Assert.AreEqual(expectedAccount.AccountNumber, actualAccount.AccountNumber);
            Assert.AreEqual(expectedAccount.Name, actualAccount.Name);
        }

        [TestMethod()]
        public void RetrieveTByIdTest()
        {
            // Setup
            const string expectedAccountNumber = "42";

            var expectedAccount = new Account()
            {
                Id = Guid.NewGuid(),
                AccountNumber = expectedAccountNumber,
                Name = "FixRM"
            };

            var context = new XrmFakedContext();
            context.Initialize(expectedAccount);

            var service = context.GetOrganizationService();

            // Act
            var actualAccount = service.Retrieve<Account>(
                id: expectedAccount.Id,
                columns: c => new { c.AccountNumber, c.Name });

            // Assert
            Assert.AreEqual(expectedAccount.Id, actualAccount.Id);
            Assert.AreEqual(expectedAccount.AccountNumber, actualAccount.AccountNumber);
            Assert.AreEqual(expectedAccount.Name, actualAccount.Name);
        }
    }
}
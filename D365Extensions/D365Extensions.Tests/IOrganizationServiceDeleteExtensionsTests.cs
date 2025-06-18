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
    public class IOrganizationServiceDeleteExtensionsTests
    {
        [TestMethod()]
        public void DeleteTByKeyAndIdPassedAsReferenceTest()
        {
            // Setup
            const string expectedAccountNumber = "42";

            var expectedAccount1 = new Account()
            {
                Id = Guid.NewGuid(),
                Name = "FixRM"
            };

            var expectedAccount2 = new Account()
            {
                Id = Guid.NewGuid(),
                AccountNumber = expectedAccountNumber,
                Name = "FixRM"
            };

            EntityReference reference1 = new EntityReference<Account>(expectedAccount1.Id);
            EntityReference reference2 = new EntityReference<Account>(a => a.AccountNumber, expectedAccountNumber);

            var context = new XrmFakedContext();
            context.Initialize([expectedAccount1, expectedAccount2]);

            context.InitializeKeyMetadata(entityLogicalName: Account.EntityLogicalName,
                keyAttributeNames: [LogicalName.GetName<Account>(a => a.AccountNumber)]);

            var service = context.GetOrganizationService();

            // Act
            service.Delete(reference1);
            service.Delete(reference2);

            // Assert
            var accounts = service.RetrieveMultiple(new QueryExpression(Account.EntityLogicalName));

            Assert.AreEqual(0, accounts.Entities.Count);
        }

        [TestMethod()]
        public void DeleteTByIdTest()
        {
            // Setup
            var account = new Account()
            {
                Id = Guid.NewGuid(),
            };

            var context = new XrmFakedContext();
            context.Initialize(account);

            var service = context.GetOrganizationService();

            // Act
            service.Delete<Account>(account.Id);

            // Assert
            var accounts = service.RetrieveMultiple(new QueryExpression(Account.EntityLogicalName));

            Assert.AreEqual(0, accounts.Entities.Count);
        }


        [TestMethod()]
        public void DeleteTByKeyNameTest()
        {
            // Setup
            string keyName = LogicalName.GetName<Account>(a => a.AccountNumber);
            const string keyValue = "42";

            var account = new Account()
            {
                Id = Guid.NewGuid(),
                AccountNumber = keyValue
            };

            var context = new XrmFakedContext();
            context.Initialize(account);

            context.InitializeKeyMetadata(entityLogicalName: Account.EntityLogicalName,
                keyAttributeNames: [keyName]);

            var service = context.GetOrganizationService();

            // Act
            service.Delete<Account>(keyName, keyValue);

            // Assert
            var accounts = service.RetrieveMultiple(new QueryExpression(Account.EntityLogicalName));

            Assert.AreEqual(0, accounts.Entities.Count);
        }

        [TestMethod()]
        public void DeleteTByLambdaKeyTest()
        {
            // Setup
            const string keyValue = "42";

            var account = new Account()
            {
                Id = Guid.NewGuid(),
                AccountNumber = keyValue
            };

            var context = new XrmFakedContext();
            context.Initialize(account);

            context.InitializeKeyMetadata(entityLogicalName: Account.EntityLogicalName,
                keyAttributeNames: [LogicalName.GetName<Account>(a => a.AccountNumber)]);

            var service = context.GetOrganizationService();

            // Act
            service.Delete<Account>(a => a.AccountNumber, keyValue);

            // Assert
            var accounts = service.RetrieveMultiple(new QueryExpression(Account.EntityLogicalName));

            Assert.AreEqual(0, accounts.Entities.Count);
        }
    }
}
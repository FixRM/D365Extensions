using D365Extensions;
using D365Extensions.Tests.Entities;
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
    public class EntityCollectionExtensionsTests
    {
        [TestMethod()]
        public void ContainsAddressTest()
        {
            // Setup
            var expectedEmail1 = "artem@grunin.ru";
            var expectedEmail2 = "doesnt@matter.com";

            var party1 = new ActivityParty()
            {
                AddressUsed = expectedEmail1
            };

            var party2 = new ActivityParty()
            {
                AddressUsed = expectedEmail2.ToUpper()
            };

            var collection = new EntityCollection();
            collection.EntityName = ActivityParty.EntityLogicalName;
            collection.Entities.Add(party1);
            collection.Entities.Add(party2);

            // Act + Assert
            Assert.IsTrue(collection.ContainsAddress(expectedEmail1));
            Assert.IsTrue(collection.ContainsAddress(expectedEmail2));
        }

        [TestMethod()]
        public void ContainsAddressShouldThrowTest()
        {
            var collection = new EntityCollection();
            collection.EntityName = Account.EntityLogicalName;

            var error = Assert.ThrowsException<ArgumentException>(()=> collection.ContainsAddress("any"));
            Assert.AreEqual(CheckParam.InvalidCollectionMessage, error.Message);
        }

        [TestMethod()]
        public void InDomainTest()
        {
            // Setup
            var expectedDomain = "grunin.ru";

            var party1 = new ActivityParty()
            {
                Id = Guid.NewGuid(),
                AddressUsed = $"artem@{expectedDomain}"
            };

            var party2 = new ActivityParty()
            {
                Id = Guid.NewGuid(),
                AddressUsed = "doesnt@matter.com"
            };

            var collection = new EntityCollection();
            collection.EntityName = ActivityParty.EntityLogicalName;
            collection.Entities.Add(party1);
            collection.Entities.Add(party2);

            // Act
            var inDomainParties = collection.GetPartiesInDomain(expectedDomain);

            // Assert
            Assert.AreEqual(ActivityParty.EntityLogicalName, inDomainParties.EntityName);
            Assert.AreEqual(1, inDomainParties.Entities.Count);
            Assert.AreEqual(party1.Id, inDomainParties.Entities[0].Id);
        }

        [TestMethod()]
        public void InDomainShouldThrowTest()
        {
            var collection = new EntityCollection();
            collection.EntityName = Account.EntityLogicalName;

            var error = Assert.ThrowsException<ArgumentException>(() => collection.GetPartiesInDomain("any"));
            Assert.AreEqual(CheckParam.InvalidCollectionMessage, error.Message);
        }

        [TestMethod()]
        public void NotInDomainTest()
        {
            // Setup
            var expectedDomain = "grunin.ru";

            var party1 = new ActivityParty()
            {
                Id = Guid.NewGuid(),
                AddressUsed = $"artem@{expectedDomain}"
            };

            var party2 = new ActivityParty()
            {
                Id = Guid.NewGuid(),
                AddressUsed = "doesnt@matter.com"
            };

            var collection = new EntityCollection();
            collection.EntityName = ActivityParty.EntityLogicalName;
            collection.Entities.Add(party1);
            collection.Entities.Add(party2);

            // Act
            var notInDomainParties = collection.GetPartiesNotInDomain(expectedDomain);

            // Assert
            Assert.AreEqual(ActivityParty.EntityLogicalName, notInDomainParties.EntityName);
            Assert.AreEqual(1, notInDomainParties.Entities.Count);
            Assert.AreEqual(party2.Id, notInDomainParties.Entities[0].Id);
        }

        [TestMethod()]
        public void NotInDomainShouldThrowTest()
        {
            var collection = new EntityCollection();
            collection.EntityName = Account.EntityLogicalName;

            var error = Assert.ThrowsException<ArgumentException>(() => collection.GetPartiesNotInDomain("any"));
            Assert.AreEqual(CheckParam.InvalidCollectionMessage, error.Message);
        }
    }
}
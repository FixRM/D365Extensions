using D365Extensions.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = D365Extensions.Tests.Entities.Task;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class RelationshipTests
    {
        [TestMethod()]
        public void Cast_Test()
        {
            //Setup
            var relationshipT = new Relationship<Account>(a=> a.Referencedaccount_master_account);

            //Act
            Relationship relationship = relationshipT;

            //Assert
            Assert.AreEqual("account_master_account", relationship.SchemaName);
            Assert.AreEqual(EntityRole.Referenced, relationship.PrimaryEntityRole);
        }

        [TestMethod()]
        public void Null_Cast_Test()
        {
            //Setup
            Relationship<Account> relationshipT = null;

            //Act
            Relationship relationship = relationshipT;

            //Assert
            Assert.IsNull(relationship);
        }

        [TestMethod()]
        public void Equals_Test()
        {
            //Setup
            var relationship1 = new Relationship<Account>(a => a.Referencedaccount_master_account);
            var relationship2 = new Relationship<Account>(a => a.Referencedaccount_master_account);

            //Act + Assert
            Assert.IsTrue(relationship1.Equals(relationship2));
            Assert.IsFalse(relationship1.Equals((Relationship) relationship2));
        }

        [TestMethod()]
        public void Not_Equals_Test()
        {
            //Setup
            var relationship1 = new Relationship<Account>(a => a.Account_Tasks);
            var relationship2 = new Relationship<Task>(t => t.Account_Tasks);

            //Act + Assert
            Assert.IsFalse(relationship1.Equals(relationship2));
            Assert.IsFalse(relationship1.Equals((Relationship)relationship2));
        }

        [TestMethod()]
        public void GetHashCode_Test()
        {
            //Setup
            Relationship<Account> relationshipT = new Relationship<Account>(a => a.Account_Tasks);
            Relationship relationship = relationshipT;

            //Act + Assert
            Assert.AreEqual(Task.EntityLogicalName.GetHashCode() ^ relationship.GetHashCode(), relationshipT.GetHashCode());
        }

        [TestMethod()]
        public void ToString_Test()
        {
            //Setup
            var relationshipT = new Relationship<Account>(a => a.Referencedaccount_master_account);
            var relationship = relationshipT;

            //Act + Assert
            Assert.AreEqual(relationship.ToString(), relationshipT.ToString());
        }
    }
}
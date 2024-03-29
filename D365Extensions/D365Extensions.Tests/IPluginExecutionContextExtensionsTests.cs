﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class IPluginExecutionContextExtensionsTests
    {
        [TestMethod]
        [Description("Should throw if message name is not Associate or Disassociate")]
        public void GetRelatedWrongMessageTest()
        {
            /// Arrange
            EntityReference account1 = new EntityReference();
            account1.LogicalName = "account";
            account1.Id = Guid.NewGuid();

            EntityReference account2 = new EntityReference();
            account2.LogicalName = "account";
            account2.Id = Guid.NewGuid();

            EntityReference contact1 = new EntityReference();
            contact1.LogicalName = "contact";
            contact1.Id = Guid.NewGuid();

            EntityReference contact2 = new EntityReference();
            contact2.LogicalName = "contact";
            contact2.Id = Guid.NewGuid();

            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add("Target", contact1);
            inputParameters.Add("RelatedEntities", new EntityReferenceCollection() { account1, account2 });

            TestPluginExecutionContext context = new TestPluginExecutionContext();
            context.InputParameters = inputParameters;
            context.MessageName = "Wrong";

            /// Act, Assert
            Assert.ThrowsException<InvalidOperationException>(() => { context.GetRelatedEntitiesByTarget(account1.LogicalName, contact1.LogicalName); });
        }

        [TestMethod]
        [Description("Should return empty collection if message is for unexpected pair of entities")]
        public void GetRelatedWrongTypeTest()
        {
            /// Arrange
            EntityReference account1 = new EntityReference();
            account1.LogicalName = "account";
            account1.Id = Guid.NewGuid();

            EntityReference account2 = new EntityReference();
            account2.LogicalName = "account";
            account2.Id = Guid.NewGuid();

            EntityReference contact1 = new EntityReference();
            contact1.LogicalName = "contact";
            contact1.Id = Guid.NewGuid();

            EntityReference contact2 = new EntityReference();
            contact2.LogicalName = "contact";
            contact2.Id = Guid.NewGuid();

            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add("Target", contact1);
            inputParameters.Add("RelatedEntities", new EntityReferenceCollection() { account1, account2 });

            TestPluginExecutionContext context = new TestPluginExecutionContext();
            context.InputParameters = inputParameters;
            context.MessageName = "Associate";

            /// Act, Assert
            var dic1 = context.GetRelatedEntitiesByTarget(account1.LogicalName, "incident");
            Assert.AreEqual(0, dic1.Count);

            dic1 = context.GetRelatedEntitiesByTarget(contact1.LogicalName, "incident");
            Assert.AreEqual(0, dic1.Count);

            dic1 = context.GetRelatedEntitiesByTarget("incident", contact1.LogicalName);
            Assert.AreEqual(0, dic1.Count);

            dic1 = context.GetRelatedEntitiesByTarget("incident", account1.LogicalName);
            Assert.AreEqual(0, dic1.Count);

            dic1 = context.GetRelatedEntitiesByTarget("incident", "product");
            Assert.AreEqual(0, dic1.Count);
        }

        [TestMethod]
        [Description("Should work if entity one is associated with array of entity two")]
        public void GetRelatedDirectTest()
        {
            /// Arrange
            EntityReference account1 = new EntityReference();
            account1.LogicalName = "account";
            account1.Id = Guid.NewGuid();

            EntityReference account2 = new EntityReference();
            account2.LogicalName = "account";
            account2.Id = Guid.NewGuid();

            EntityReference contact1 = new EntityReference();
            contact1.LogicalName = "contact";
            contact1.Id = Guid.NewGuid();

            EntityReference contact2 = new EntityReference();
            contact2.LogicalName = "contact";
            contact2.Id = Guid.NewGuid();

            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add("Target", account1);
            inputParameters.Add("RelatedEntities", new EntityReferenceCollection() { contact1, contact2 });

            TestPluginExecutionContext context = new TestPluginExecutionContext();
            context.InputParameters = inputParameters;
            context.MessageName = "Associate";

            /// Act
            var dic = context.GetRelatedEntitiesByTarget(account1.LogicalName, contact1.LogicalName);

            /// Assert
            Assert.AreEqual(1, dic.Count);
            Assert.IsTrue(dic.Keys.Contains(account1));

            EntityReferenceCollection value = dic[account1];
            Assert.AreEqual(2, value.Count);
            Assert.IsTrue(value.Contains(contact1));
            Assert.IsTrue(value.Contains(contact2));
        }

        [TestMethod]
        [Description("Should work if entity two is associated with array of entity one")]
        public void GetRelatedInvertedTest()
        {
            /// Arrange
            EntityReference account1 = new EntityReference();
            account1.LogicalName = "account";
            account1.Id = Guid.NewGuid();

            EntityReference account2 = new EntityReference();
            account2.LogicalName = "account";
            account2.Id = Guid.NewGuid();

            EntityReference contact1 = new EntityReference();
            contact1.LogicalName = "contact";
            contact1.Id = Guid.NewGuid();

            EntityReference contact2 = new EntityReference();
            contact2.LogicalName = "contact";
            contact2.Id = Guid.NewGuid();

            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add("Target", contact1);
            inputParameters.Add("RelatedEntities", new EntityReferenceCollection() { account1, account2 });

            TestPluginExecutionContext context = new TestPluginExecutionContext();
            context.InputParameters = inputParameters;
            context.MessageName = "Associate";

            /// Act
            var dic = context.GetRelatedEntitiesByTarget(account1.LogicalName, contact1.LogicalName);

            /// Assert
            Assert.AreEqual(2, dic.Count);
            Assert.IsTrue(dic.Keys.Contains(account1));
            Assert.IsTrue(dic.Keys.Contains(account2));

            EntityReferenceCollection value1 = dic[account1];
            Assert.AreEqual(1, value1.Count);
            Assert.IsTrue(value1.Contains(contact1));

            EntityReferenceCollection value2 = dic[account2];
            Assert.AreEqual(1, value2.Count);
            Assert.IsTrue(value2.Contains(contact1));
        }

        [TestMethod()]
        public void GetInputParameterTest()
        {
            int? structType = 42;
            string referenceType = "ultimate question of life the universe and everything";

            TestPluginExecutionContext context = new TestPluginExecutionContext();
            context.InputParameters = new ParameterCollection();
            context.InputParameters.Add("struct", structType);
            context.InputParameters.Add("class", referenceType);

            Assert.AreEqual<int?>(structType, context.GetInputParameter<int?>("struct"));
            Assert.AreEqual<string>(referenceType, context.GetInputParameter<string>("class"));
        }

        [TestMethod()]
        public void GetOrganizationTest()
        {
            // Setup
            var expectedOrgId = Guid.NewGuid();

            TestPluginExecutionContext context = new TestPluginExecutionContext();
            context.OrganizationId = expectedOrgId;

            // Act
            var actualOrgId = context.GetOrganization();

            // Assert
            Assert.AreEqual(expectedOrgId, actualOrgId.Id);
            Assert.AreEqual("organization", actualOrgId.LogicalName);
        }

        [TestMethod()]
        public void GetPrimaryEntityTest()
        {
            // Setup
            var expectedEntityRef = new EntityReference()
            {
                Id = Guid.NewGuid(),
                LogicalName = "account"
            };

            TestPluginExecutionContext context = new TestPluginExecutionContext();
            context.PrimaryEntityId = expectedEntityRef.Id;
            context.PrimaryEntityName = expectedEntityRef.LogicalName;

            // Act
            var actualEntityRef = context.GetPrimaryEntity();

            // Assert
            Assert.AreEqual(expectedEntityRef.Id, actualEntityRef.Id);
            Assert.AreEqual(expectedEntityRef.LogicalName, actualEntityRef.LogicalName);
        }

        [TestMethod()]
        public void GetPrimaryEntity_No_Primary_Id_Test()
        {
            // Setup
            TestPluginExecutionContext context = new TestPluginExecutionContext();

            // Act
            var actualEntityRef = context.GetPrimaryEntity();

            // Assert
            Assert.IsNull(actualEntityRef);            
        }

        [TestMethod()]
        public void GetUserTest()
        {
            // Setup
            var expectedUserRef = new EntityReference()
            {
                Id = Guid.NewGuid(),
                LogicalName = "systemuser"
            };

            TestPluginExecutionContext context = new TestPluginExecutionContext();
            context.UserId = expectedUserRef.Id;

            // Act
            var actualUserRef = context.GetUser();

            // Assert
            Assert.AreEqual(expectedUserRef.Id, actualUserRef.Id);
            Assert.AreEqual(expectedUserRef.LogicalName, actualUserRef.LogicalName);
        }

        [TestMethod()]
        public void GetInitiatingUserTest()
        {
            // Setup
            var expectedUserRef = new EntityReference()
            {
                Id = Guid.NewGuid(),
                LogicalName = "systemuser"
            };

            TestPluginExecutionContext context = new TestPluginExecutionContext();
            context.InitiatingUserId = expectedUserRef.Id;

            // Act
            var actualUserRef = context.GetInitiatingUser();

            // Assert
            Assert.AreEqual(expectedUserRef.Id, actualUserRef.Id);
            Assert.AreEqual(expectedUserRef.LogicalName, actualUserRef.LogicalName);
        }

        [TestMethod()]
        public void GetBusinessUnitTest()
        {
            // Setup
            var expectedUnitRef = new EntityReference()
            {
                Id = Guid.NewGuid(),
                LogicalName = "businessunit"
            };

            TestPluginExecutionContext context = new TestPluginExecutionContext();
            context.BusinessUnitId= expectedUnitRef.Id;

            // Act
            var actualUnitRef = context.GetBusinessUnit();

            // Assert
            Assert.AreEqual(expectedUnitRef.Id, actualUnitRef.Id);
            Assert.AreEqual(expectedUnitRef.LogicalName, actualUnitRef.LogicalName);
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeXrmEasy;

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

            var context = new XrmFakedPluginExecutionContext();
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

            var context = new XrmFakedPluginExecutionContext();
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

            var context = new XrmFakedPluginExecutionContext();
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

            var context = new XrmFakedPluginExecutionContext();
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

            var context = new XrmFakedPluginExecutionContext();
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

            var context = new XrmFakedPluginExecutionContext();
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

            var context = new XrmFakedPluginExecutionContext();
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
            var context = new XrmFakedPluginExecutionContext();

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

            var context = new XrmFakedPluginExecutionContext();
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

            var context = new XrmFakedPluginExecutionContext();
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

            var context = new XrmFakedPluginExecutionContext();
            context.BusinessUnitId= expectedUnitRef.Id;

            // Act
            var actualUnitRef = context.GetBusinessUnit();

            // Assert
            Assert.AreEqual(expectedUnitRef.Id, actualUnitRef.Id);
            Assert.AreEqual(expectedUnitRef.LogicalName, actualUnitRef.LogicalName);
        }

        [TestMethod()]
        public void GetRelatedEntitiesAsTuplesWrongMessageTest()
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

            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add("Target", contact1);
            inputParameters.Add("RelatedEntities", new EntityReferenceCollection() { account1, account2 });

            var context = new XrmFakedPluginExecutionContext();
            context.InputParameters = inputParameters;
            context.MessageName = "Wrong";

            var expectedError = string.Format(IPluginExecutionContextExtensions.WrongMessageForGetRelatedEntities, context.MessageName);

            /// Act, Assert
            var error = Assert.ThrowsException<InvalidOperationException>(() => context.GetRelatedEntitiesAsTuples(account1.LogicalName, contact1.LogicalName));

            Assert.AreEqual(expectedError, error.Message);
        }

        [TestMethod]
        [Description("Should return empty collection if message is for unexpected pair of entities")]
        public void GetRelatedEntitiesAsTuplesWrongTypeTest()
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

            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add("Target", contact1);
            inputParameters.Add("RelatedEntities", new EntityReferenceCollection() { account1, account2 });

            var context = new XrmFakedPluginExecutionContext();
            context.InputParameters = inputParameters;
            context.MessageName = "Associate";

            /// Act, Assert
            var result = context.GetRelatedEntitiesAsTuples(account1.LogicalName, "incident");
            Assert.AreEqual(0, result.Count);

            result = context.GetRelatedEntitiesAsTuples(contact1.LogicalName, "incident");
            Assert.AreEqual(0, result.Count);

            result = context.GetRelatedEntitiesAsTuples("incident", contact1.LogicalName);
            Assert.AreEqual(0, result.Count);

            result = context.GetRelatedEntitiesAsTuples("incident", account1.LogicalName);
            Assert.AreEqual(0, result.Count);

            result = context.GetRelatedEntitiesAsTuples("incident", "product");
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [Description("Should work if entity one is associated with array of entity two")]
        public void GetRelatedEntitiesAsTuplesDirectTest()
        {
            /// Arrange
            EntityReference account1 = new EntityReference();
            account1.LogicalName = "account";
            account1.Id = Guid.NewGuid();

            EntityReference contact1 = new EntityReference();
            contact1.LogicalName = "contact";
            contact1.Id = Guid.NewGuid();

            EntityReference contact2 = new EntityReference();
            contact2.LogicalName = "contact";
            contact2.Id = Guid.NewGuid();

            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add("Target", account1);
            inputParameters.Add("RelatedEntities", new EntityReferenceCollection() { contact1, contact2 });

            var context = new XrmFakedPluginExecutionContext();
            context.InputParameters = inputParameters;
            context.MessageName = "Associate";

            /// Act
            var result = context.GetRelatedEntitiesAsTuples(account1.LogicalName, contact1.LogicalName);

            /// Assert
            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(account1, result[0].Item1);
            Assert.AreEqual(contact1, result[0].Item2);

            Assert.AreEqual(account1, result[1].Item1);
            Assert.AreEqual(contact2, result[1].Item2);
        }

        [TestMethod]
        [Description("Should work if entity two is associated with array of entity one")]
        public void GetRelatedEntitiesAsTuplesInvertedTest()
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

            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add("Target", contact1);
            inputParameters.Add("RelatedEntities", new EntityReferenceCollection() { account1, account2 });

            var context = new XrmFakedPluginExecutionContext();
            context.InputParameters = inputParameters;
            context.MessageName = "Associate";

            /// Act
            var result = context.GetRelatedEntitiesAsTuples(account1.LogicalName, contact1.LogicalName);

            /// Assert
            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(account1, result[0].Item1);
            Assert.AreEqual(contact1, result[0].Item2);

            Assert.AreEqual(account2, result[1].Item1);
            Assert.AreEqual(contact1, result[1].Item2);
        }

        [TestMethod()]
        public void GetPreTargetTest()
        {
            // Setup
            var target = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = "account",
                ["name"] = "FixRM Corp",
                ["foundationdate"] = new DateTime(1985, 8, 8)
            };
            target.FormattedValues["foundationdate"] = "08.08.1985";

            var preImage = new Entity()
            {
                Id = target.Id,
                LogicalName = target.LogicalName,
                ["name"] = "FixRM",
                ["statecode"] = new OptionSetValue(0)
            };
            preImage.FormattedValues["statecode"] = "Active";

            var context = new XrmFakedPluginExecutionContext();
            context.InputParameters = new ParameterCollection
            {
                { "Target", target }
            };
            context.PreEntityImages = new EntityImageCollection
            {
                { "PreImage", preImage }
            };

            // Act
            var preTarget = context.GetPreTarget();

            // Assert
            Assert.AreNotEqual(preTarget, target);

            Assert.AreEqual(preTarget.Id, target.Id);
            Assert.AreEqual(preTarget.LogicalName, target.LogicalName);
            Assert.AreEqual(preTarget["name"], target["name"]);
            Assert.AreEqual(preTarget["foundationdate"], target["foundationdate"]);
            Assert.AreEqual(preTarget["statecode"], preImage["statecode"]);

            Assert.AreEqual(preTarget.FormattedValues["foundationdate"], target.FormattedValues["foundationdate"]);
            Assert.AreEqual(preTarget.FormattedValues["statecode"], preImage.FormattedValues["statecode"]);
        }

        [TestMethod()]
        public void GetPostTargetTest()
        {
            // Setup
            var target = new Entity()
            {
                Id = Guid.NewGuid(),
                LogicalName = "account",
                ["name"] = "FixRM Corp",
                ["foundationdate"] = new DateTime(1985, 8, 8)
            };
            target.FormattedValues["foundationdate"] = "08.08.1985";

            var postImage = new Entity()
            {
                Id = target.Id,
                LogicalName = target.LogicalName,
                ["code"] = "1985-000001-XYZ",
                ["statecode"] = new OptionSetValue(0)
            };
            postImage.FormattedValues["statecode"] = "Active";

            var context = new XrmFakedPluginExecutionContext();
            context.InputParameters = new ParameterCollection
            {
                { "Target", target }
            };
            context.PreEntityImages = new EntityImageCollection
            {
                { "PreImage", postImage }
            };

            // Act
            var preTarget = context.GetPreTarget();

            // Assert
            Assert.AreNotEqual(preTarget, target);

            Assert.AreEqual(preTarget.Id, target.Id);
            Assert.AreEqual(preTarget.LogicalName, target.LogicalName);
            Assert.AreEqual(preTarget["name"], target["name"]);
            Assert.AreEqual(preTarget["foundationdate"], target["foundationdate"]);
            Assert.AreEqual(preTarget["code"], postImage["code"]);
            Assert.AreEqual(preTarget["statecode"], postImage["statecode"]);

            Assert.AreEqual(preTarget.FormattedValues["foundationdate"], target.FormattedValues["foundationdate"]);
            Assert.AreEqual(preTarget.FormattedValues["statecode"], postImage.FormattedValues["statecode"]);
        }
    }
}
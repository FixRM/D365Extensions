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
    }
}
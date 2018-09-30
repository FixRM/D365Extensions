using MsCrmExtensions;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;

namespace MsCrmExtensions.Tests
{
    [TestClass]
    public class EntityExtensionsTests
    {
        [TestMethod()]
        public void GetFormatedValueTest()
        {
            /// Setup test data
            String attributeLogicalName = "statuscode";
            String attributeValue = "active";
            OptionSetValue statusCodeValue = new OptionSetValue(0);

            /// Create entity
            Entity entity = new Entity();
            entity.Attributes.Add(attributeLogicalName, statusCodeValue);
            entity.FormattedValues.Add(attributeLogicalName, attributeValue);

            String formatedValue;

            /// Test for required parameters
            try
            {
                formatedValue = entity.GetFormatedValue(null);
            }
            catch (ArgumentNullException e)
            {
                /// Ok
                Assert.AreEqual<String>(e.ParamName, "attributeLogicalName");
            }

            /// Test for exisiting attribute
            formatedValue = entity.GetFormatedValue(attributeLogicalName);
            Assert.AreEqual<String>(attributeValue, formatedValue);

            /// Test for not exisiting attribute
            formatedValue = entity.GetFormatedValue("doesntexist");
            Assert.IsNull(formatedValue);
        }

        [TestMethod()]
        public void GetAliasedValueTest()
        {
            /// Setup test data
            String mainEntityLogicalName = "account";
            String linkedEntityLogicalName = "contact";

            String attributeLogicalName1 = "name";
            String value1 = "Grunin Artem";
            AliasedValue aliasedValue1 = new AliasedValue(linkedEntityLogicalName, attributeLogicalName1, value1);

            String attributeLogicalName2 = "birthdate";
            DateTime? value2 = new DateTime(1985, 8, 8);
            AliasedValue aliasedValue2 = new AliasedValue(linkedEntityLogicalName, attributeLogicalName2, value2);

            /// Create Entity
            String alias = "ac";
            String aliasedName1 = $"{alias}.{attributeLogicalName1}";
            String aliasedName2 = $"{alias}.{attributeLogicalName2}";

            /// Create main entity
            Entity entity = new Entity(mainEntityLogicalName);

            /// Add main entity attributes
            entity.Attributes.Add("name", "FixRM");
            entity.Attributes.Add("accountnumber", "1");
            entity.Attributes.Add("statecode", new OptionSetValue(0));

            /// Add linked entity attributes
            entity.Attributes.Add(aliasedName1, aliasedValue1);
            entity.Attributes.Add(aliasedName2, aliasedValue2);

            /// Test
            String actualValue;

            /// Test for required parameters
            try
            {
                actualValue = entity.GetAliasedValue<String>(null, alias);
            }
            catch (ArgumentNullException e)
            {
                /// Ok
                Assert.AreEqual<String>(e.ParamName, "attributeLogicalName");
            }

            try
            {
                actualValue = entity.GetAliasedValue<String>(attributeLogicalName1, null);
            }
            catch (ArgumentNullException e)
            {
                /// Ok
                Assert.AreEqual<String>(e.ParamName, "alias");
            }

            /// Test for exisiting attribute
            actualValue = entity.GetAliasedValue<String>(attributeLogicalName1, alias);
            Assert.IsFalse(String.IsNullOrEmpty(actualValue));
            Assert.AreEqual<String>(value1, actualValue);

            DateTime? actualValue2 = entity.GetAliasedValue<DateTime?>(attributeLogicalName2, alias);
            Assert.IsTrue(actualValue2.HasValue);
            Assert.AreEqual<DateTime?>(value2, actualValue2);

            /// Test for not exisiting attribute
            actualValue = entity.GetAliasedValue<String>("doesntexist", alias);
            Assert.IsNull(actualValue);
        }

        [TestMethod()]
        public void GetAliasedEntityTest()
        {
            /// Setup test data
            String mainEntityLogicalName = "account";
            String linkedEntityLogicalName1 = "contact";
            String linkedEntityLogicalName2 = "opportunity";

            String attributeLogicalName1 = "name";
            String value1 = "Grunin Artem";
            AliasedValue aliasedValue1 = new AliasedValue(linkedEntityLogicalName1, attributeLogicalName1, value1);

            String attributeLogicalName2 = "birthdate";
            DateTime? value2 = new DateTime(1985, 8, 8);
            AliasedValue aliasedValue2 = new AliasedValue(linkedEntityLogicalName1, attributeLogicalName2, value2);

            String attributeLogicalName3 = "name";
            String value3 = "Microsoft acquiring";
            AliasedValue aliasedValue3 = new AliasedValue(linkedEntityLogicalName2, attributeLogicalName3, value3);

            /// Create Entity
            String alias1 = "ac";
            String aliasedName1 = $"{alias1}.{attributeLogicalName1}";
            String aliasedName2 = $"{alias1}.{attributeLogicalName2}";

            String alias2 = linkedEntityLogicalName2;
            String aliasedName3 = $"{alias2}.{attributeLogicalName2}";

            /// Create main entity
            Entity entity = new Entity(mainEntityLogicalName);

            /// Add main entity attributes
            entity.Attributes.Add("name", "FixRM");
            entity.Attributes.Add("accountnumber", "1");
            entity.Attributes.Add("statecode", new OptionSetValue(0));

            /// Add linked entity attributes
            entity.Attributes.Add(aliasedName1, aliasedValue1);
            entity.Attributes.Add(aliasedName2, aliasedValue2);
            entity.Attributes.Add(aliasedName3, aliasedValue3);

            Entity actualEntity;
            /// Test for required parameters
            try
            {
                actualEntity = entity.GetAliasedEntity(null, alias1);
            }
            catch (ArgumentNullException e)
            {
                /// Ok
                Assert.AreEqual<String>(e.ParamName, "entityLogicalName");
            }

            /// Test with alias parameter
            actualEntity = entity.GetAliasedEntity(linkedEntityLogicalName1, alias1);
            Assert.IsNotNull(actualEntity);
            Assert.AreEqual<String>(actualEntity.LogicalName, linkedEntityLogicalName1);
            Assert.AreEqual<int>(2, actualEntity.Attributes.Count);

            String actualAttributeValue1 = actualEntity.GetAttributeValue<String>(attributeLogicalName1);
            Assert.IsNotNull(actualAttributeValue1);
            Assert.AreEqual<String>(value1, actualAttributeValue1);

            DateTime? actualAttributeValue2 = actualEntity.GetAttributeValue<DateTime?>(attributeLogicalName2);
            Assert.IsNotNull(actualAttributeValue2);
            Assert.AreEqual<DateTime?>(value2, actualAttributeValue2);

            /// Test without alias
            actualEntity = entity.GetAliasedEntity(linkedEntityLogicalName2);
            Assert.IsNotNull(actualEntity);
            Assert.AreEqual<String>(actualEntity.LogicalName, linkedEntityLogicalName2);
            Assert.AreEqual<int>(1, actualEntity.Attributes.Count);

            String actualAttributeValue3 = actualEntity.GetAttributeValue<String>(attributeLogicalName3);
            Assert.IsNotNull(actualAttributeValue3);
            Assert.AreEqual<String>(value3, actualAttributeValue3);

            /// Test for not existing alias
            actualEntity = entity.GetAliasedEntity("quote", "q");
            Assert.IsNotNull(actualEntity);
            Assert.AreEqual<int>(0, actualEntity.Attributes.Count);

            actualEntity = entity.GetAliasedEntity("quote");
            Assert.IsNotNull(actualEntity);
            Assert.AreEqual<int>(0, actualEntity.Attributes.Count);
        }

        [TestMethod()]
        public void MergeAttributesTest()
        {
            /// Setup test data
            Entity target = new Entity();
            target.Attributes.Add("firstname", "Artem"); //exist in both
            target.Attributes.Add("lastname", "Grunin"); //exist in both 
            target.Attributes.Add("middlename", "Igorevich"); // exist only in target

            Entity source = new Entity();
            source.Attributes.Add("firstname", "Artem the great"); //changed in source
            source.Attributes.Add("lastname", "Grunin"); //exist in both
            source.Attributes.Add("birthdate", new DateTime(1985, 8, 8)); // exist only in source

            /// Run test
            target.MergeAttributes(source);

            /// Should be one more attribute
            Assert.AreEqual(4, target.Attributes.Count);

            /// Should have original value
            String firstname = target.GetAttributeValue<String>("firstname");
            Assert.AreEqual("Artem", firstname);

            /// Test for existing attribute
            String lastname = target.GetAttributeValue<String>("lastname");
            Assert.AreEqual("Grunin", lastname);

            /// Test for new attribute 
            DateTime birthdate = target.GetAttributeValue<DateTime>("birthdate");
            Assert.AreEqual(new DateTime(1985, 8, 8), birthdate);
        }
    }
}

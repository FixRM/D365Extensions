using D365Extensions.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;

namespace D365Extensions.Tests
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

            /// Test for existing attribute
            formatedValue = entity.GetFormatedValue(attributeLogicalName);
            Assert.AreEqual<String>(attributeValue, formatedValue);

            /// Test for not existing attribute
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

            String attributeLogicalName3 = "totalamount";
            Double? value3 = 12345.67;
            AliasedValue aliasedValue3 = new AliasedValue("opportunity", attributeLogicalName3, value3);

            /// Create Entity
            String alias = "ac";
            String aliasedName1 = $"{alias}.{attributeLogicalName1}";
            String aliasedName2 = $"{alias}.{attributeLogicalName2}";
            String aliasedName3 = "total_amount_sum";

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

            /// Test
            String actualValue;

            /// Test for required parameters
            try
            {
                actualValue = entity.GetAliasedValue<String>(attributeLogicalName1, null);
                Assert.Fail();
            }
            catch (ArgumentNullException e)
            {
                /// Ok
                Assert.AreEqual<String>(e.ParamName, "alias");
            }

            /// Test for existing attribute
            actualValue = entity.GetAliasedValue<String>(attributeLogicalName1, alias);
            Assert.IsFalse(String.IsNullOrEmpty(actualValue));
            Assert.AreEqual<String>(value1, actualValue);

            DateTime? actualValue2 = entity.GetAliasedValue<DateTime?>(attributeLogicalName2, alias);
            Assert.IsTrue(actualValue2.HasValue);
            Assert.AreEqual<DateTime?>(value2, actualValue2);

            /// Test for not existing attribute
            actualValue = entity.GetAliasedValue<String>("doesntexist", alias);
            Assert.IsNull(actualValue);

            /// Test for aggregate fetch aliased values
            Double? actualAgregeteValue = entity.GetAliasedValue<Double?>(null, aliasedName3);
            Assert.AreEqual<Double?>(value3, actualAgregeteValue);
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
            String formatedValue2 = "08.08.1985";

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

            /// Add formated values
            entity.FormattedValues.Add(aliasedName2, formatedValue2);

            Entity actualEntity;
            /// Test for required parameters
            try
            {
                actualEntity = entity.GetAliasedEntity(null, alias1);
                Assert.Fail();
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

            Assert.AreEqual(formatedValue2, actualEntity.FormattedValues[attributeLogicalName2]);

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
        public void GetAliasedEntityGenericTest()
        {
            /// Setup test data
            String mainEntityLogicalName = "account";
            String linkedEntityLogicalName1 = "testentity";

            String attributeLogicalName1 = "name";
            String value1 = "Grunin Artem";
            AliasedValue aliasedValue1 = new AliasedValue(linkedEntityLogicalName1, attributeLogicalName1, value1);

            String attributeLogicalName2 = "birthdate";
            DateTime? value2 = new DateTime(1985, 8, 8);
            AliasedValue aliasedValue2 = new AliasedValue(linkedEntityLogicalName1, attributeLogicalName2, value2);

            /// Create Entity
            String alias1 = "ac";
            String aliasedName1 = $"{alias1}.{attributeLogicalName1}";
            String aliasedName2 = $"{alias1}.{attributeLogicalName2}";

            /// Create main entity
            Entity entity = new Entity(mainEntityLogicalName);

            /// Add main entity attributes
            entity.Attributes.Add("name", "FixRM");
            entity.Attributes.Add("accountnumber", "1");
            entity.Attributes.Add("statecode", new OptionSetValue(0));

            /// Add linked entity attributes
            entity.Attributes.Add(aliasedName1, aliasedValue1);
            entity.Attributes.Add(aliasedName2, aliasedValue2);

            TestEntity actualEntity;

            /// Test
#pragma warning disable CS0618 // Type or member is obsolete
            actualEntity = entity.GetAliasedEntity<TestEntity>(linkedEntityLogicalName1, alias1);
#pragma warning restore CS0618 // Type or member is obsolete

            /// Instance is correct type
            Assert.IsNotNull(actualEntity);
            Assert.IsInstanceOfType(actualEntity, typeof(TestEntity));

            /// attribute values of early bound entity are ok
            Assert.AreEqual<String>(actualEntity.LogicalName, linkedEntityLogicalName1);
            Assert.AreEqual<int>(2, actualEntity.Attributes.Count);

            String actualAttributeValue1 = actualEntity.GetAttributeValue<String>(attributeLogicalName1);
            Assert.IsNotNull(actualAttributeValue1);
            Assert.AreEqual<String>(value1, actualAttributeValue1);

            DateTime? actualAttributeValue2 = actualEntity.GetAttributeValue<DateTime?>(attributeLogicalName2);
            Assert.IsNotNull(actualAttributeValue2);
            Assert.AreEqual<DateTime?>(value2, actualAttributeValue2);

        }

        [TestMethod()]
        public void GetAliasedEntityGenericTest2()
        {
            /// Setup test data
            String mainEntityLogicalName = "account";
            String linkedEntityLogicalName1 = "testentity";

            String attributeLogicalName1 = "name";
            String value1 = "Grunin Artem";
            AliasedValue aliasedValue1 = new AliasedValue(linkedEntityLogicalName1, attributeLogicalName1, value1);

            String attributeLogicalName2 = "birthdate";
            DateTime? value2 = new DateTime(1985, 8, 8);
            AliasedValue aliasedValue2 = new AliasedValue(linkedEntityLogicalName1, attributeLogicalName2, value2);

            /// Create Entity
            String alias1 = "ac";
            String aliasedName1 = $"{alias1}.{attributeLogicalName1}";
            String aliasedName2 = $"{alias1}.{attributeLogicalName2}";

            /// Create main entity
            Entity entity = new Entity(mainEntityLogicalName);

            /// Add main entity attributes
            entity.Attributes.Add("name", "FixRM");
            entity.Attributes.Add("accountnumber", "1");
            entity.Attributes.Add("statecode", new OptionSetValue(0));

            /// Add linked entity attributes
            entity.Attributes.Add(aliasedName1, aliasedValue1);
            entity.Attributes.Add(aliasedName2, aliasedValue2);

            TestEntity actualEntity;

            /// Test
            actualEntity = entity.GetAliasedEntity<TestEntity>(alias1);

            /// Instance is correct type
            Assert.IsNotNull(actualEntity);
            Assert.IsInstanceOfType(actualEntity, typeof(TestEntity));

            /// attribute values of early bound entity are ok
            Assert.AreEqual<String>(actualEntity.LogicalName, linkedEntityLogicalName1);
            Assert.AreEqual<int>(2, actualEntity.Attributes.Count);

            String actualAttributeValue1 = actualEntity.GetAttributeValue<String>(attributeLogicalName1);
            Assert.IsNotNull(actualAttributeValue1);
            Assert.AreEqual<String>(value1, actualAttributeValue1);

            DateTime? actualAttributeValue2 = actualEntity.GetAttributeValue<DateTime?>(attributeLogicalName2);
            Assert.IsNotNull(actualAttributeValue2);
            Assert.AreEqual<DateTime?>(value2, actualAttributeValue2);
        }

        [TestMethod()]
        public void GetAliasedEntityPrefixTest()
        {
            /// Setup
            Entity entity = new Entity()
            {
                ["name"] = "FixRM",
                ["accountid"] = new EntityReference("account", Guid.NewGuid()),
            };

            /// Act
            Entity aliasedEntity1 = entity.GetAliasedEntity("account", "na");
            Entity aliasedEntity2 = entity.GetAliasedEntity("account");

            /// Assert "name" not matched with prefix "na" or "account
            Assert.IsNull(aliasedEntity1.GetAttributeValue<string>("accountid"));
            Assert.IsNull(aliasedEntity2.GetAttributeValue<string>("name"));
        }

        [TestMethod()]
        public void MergeAttributesTest()
        {
            /// Setup
            EntityReference accountId = new EntityReference()
            {
                Id = Guid.NewGuid(),
                LogicalName = "account",
                Name = "FixRM"
            };

            Entity target = new Entity();
            target.Attributes.Add("firstname", "Artem The Great"); //changed in target
            target.Attributes.Add("middlename", "Igorevich"); //added in target

            target.Attributes.Add("statuscode", new OptionSetValue(2)); //changed in target
            target.FormattedValues.Add("statuscode", "Confirmed"); //changed in target

            // entity image
            Entity source = new Entity();
            source.Attributes.Add("firstname", "Artem"); //original value
            source.Attributes.Add("lastname", "Grunin");
            source.Attributes.Add("accountid", accountId);

            source.Attributes.Add("birthdate", new DateTime(1985, 8, 8));
            source.Attributes.Add("statecode", new OptionSetValue(0));
            source.Attributes.Add("statuscode", new OptionSetValue(1));

            source.FormattedValues.Add("birthdate", "1985-08-08Z");
            source.FormattedValues.Add("statecode", "Active");
            source.FormattedValues.Add("statuscode", "Active");

            /// Act
            target.MergeAttributes(source);

            /// Assert
            Assert.AreEqual(7, target.Attributes.Count);

            var firstname = target.GetAttributeValue<string>("firstname");
            Assert.AreEqual("Artem The Great", firstname);

            var lastname = target.GetAttributeValue<string>("lastname");
            Assert.AreEqual("Grunin", lastname);

            var middlename = target.GetAttributeValue<string>("middlename");
            Assert.AreEqual("Igorevich", middlename);

            var statecode = target.GetAttributeValue<OptionSetValue>("statecode");
            Assert.AreEqual(0, statecode.Value);

            var statuscode = target.GetAttributeValue<OptionSetValue>("statuscode");
            Assert.AreEqual(2, statuscode.Value);

            DateTime birthdate = target.GetAttributeValue<DateTime>("birthdate");
            Assert.AreEqual(new DateTime(1985, 8, 8), birthdate);

            EntityReference actualAccountId = target.GetAttributeValue<EntityReference>("accountid");
            Assert.AreEqual(actualAccountId, accountId);
            Assert.AreEqual(actualAccountId.LogicalName, accountId.LogicalName);
            Assert.AreEqual(actualAccountId.Name, accountId.Name);

            Assert.AreEqual(3, target.FormattedValues.Count);

            Assert.AreEqual("Confirmed", target.FormattedValues["statuscode"]);
            Assert.AreEqual("Active", target.FormattedValues["statecode"]);
            Assert.AreEqual("1985-08-08Z", target.FormattedValues["birthdate"]);
        }

        [TestMethod()]
        public void MergeAttributesNewRefsTest()
        {
            /// Setup test data
            EntityReference accountId = new EntityReference()
            {
                Id = Guid.NewGuid(),
                LogicalName = "account",
                Name = "FixRM"
            };

            Entity target = new Entity();

            Entity source = new Entity();

            source.Attributes.Add("accountid", accountId);

            /// Act
            target.MergeAttributes(source, newRefs: true);

            /// Test for EntityReference attribute
            EntityReference actualAccountId = target.GetAttributeValue<EntityReference>("accountid");
            Assert.AreEqual(actualAccountId, accountId);
            Assert.AreEqual(actualAccountId.LogicalName, accountId.LogicalName);
            Assert.IsNull(actualAccountId.Name);
        }

        [TestMethod()]
        public void SetValueTest()
        {
            /// Setup test data 
            String existingAttributeLogicalName = "name";
            String existingAttributeValue = "Artem Grunin";

            String nonexistentAttributeLogicalName = "birthdate";
            DateTime? nonexistentAttributeValue = new DateTime(1985, 8, 8);

            Entity entity = new Entity();
            entity.Attributes.Add(existingAttributeLogicalName, existingAttributeValue);

            /// Act: Set existing attribute value
            String expectedExistingAttributeValue = "Mr Artem Grunin";
            entity.SetAttributeValue(existingAttributeLogicalName, expectedExistingAttributeValue);

            /// Act: Set nonexistent attribute value
            entity.SetAttributeValue(nonexistentAttributeLogicalName, nonexistentAttributeValue);

            /// Assert: Should add one more attributes
            Assert.AreEqual<int>(2, entity.Attributes.Count);

            /// Assert: Should update existing attribute value
            Assert.AreEqual(expectedExistingAttributeValue, entity[existingAttributeLogicalName]);

            /// Assert: Should add nonexistent attribute value
            Assert.AreEqual(nonexistentAttributeValue, entity[nonexistentAttributeLogicalName]);
        }

        [TestMethod()]
        public void ToEntityReferenceTest()
        {
            /// Setup
            Entity entity = new Entity("account", "key", "value");

            /// Act: OOB method
            EntityReference reference = entity.ToEntityReference();

            /// Act: Extension method
            EntityReference keyReference = entity.ToEntityReference(true);

            /// Assert: OOB Method
            Assert.AreEqual(0, reference.KeyAttributes.Count);

            /// Assert: Extension Method
            Assert.AreEqual(1, keyReference.KeyAttributes.Count);

            Assert.AreEqual("value", keyReference.KeyAttributes["key"]);
        }

        [TestMethod()]
        public void ToTraceStringTest()
        {
            // Setup
            Entity entity = new Entity("account");

            string expectedTraceString =
                $$"""
                Entity { LogicalName = "account", Id = "{00000000-0000-0000-0000-000000000000}" }
                Attributes: AttributeCollection { Count = 0 }
                """;

            // Act
            string actualTraceString = entity.ToTraceString();

            // Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void ByteArraysAreEqualTest()
        {
            //Setup
            byte[] b1 = null;
            byte[] b2 = null;

            //Act + Assert
            Assert.IsTrue(EntityExtensions.AreEqual(b1, b2));

            b1 = new byte[] { 1, 2, 3 };
            Assert.IsFalse(EntityExtensions.AreEqual(b1, b2));

            b2 = new byte[] { 1, 2, 3 };
            Assert.IsTrue(EntityExtensions.AreEqual(b1, b2));

            b2 = new byte[] { 1, 2, 4 };
            Assert.IsFalse(EntityExtensions.AreEqual(b1, b2));

            b1 = null;
            Assert.IsFalse(EntityExtensions.AreEqual(b1, b2));
        }

        [TestMethod()]
        public void CollectionsAreEqualTest()
        {
            //Setup
            EntityCollection c1 = null;
            EntityCollection c2 = null;

            //Act + Assert
            Assert.IsTrue(EntityExtensions.AreEqual(c1, c2));

            c1 = new EntityCollection();
            Assert.IsFalse(EntityExtensions.AreEqual(c1, c2));

            c2 = new EntityCollection();
            Assert.IsTrue(EntityExtensions.AreEqual(c1, c2));

            c1.Entities.Add(new Entity() { Id = Guid.Parse("{489B9C29-73AD-4145-AEA0-E6CF03CB82DF}") });
            Assert.IsFalse(EntityExtensions.AreEqual(c1, c2));

            c2.Entities.Add(new Entity() { Id = Guid.Parse("{489B9C29-73AD-4145-AEA0-E6CF03CB82DF}") });
            Assert.IsTrue(EntityExtensions.AreEqual(c1, c2));

            c2.Entities.Clear();
            c2.Entities.Add(new Entity() { Id = Guid.Parse("{489B9C29-73AD-4145-AEA0-E6CF03CB82DA}") });
            Assert.IsFalse(EntityExtensions.AreEqual(c1, c2));

            c1.Entities.Add(new Entity() { Id = Guid.Parse("{77E2CB34-A621-4CA9-844D-F484A0458D2F}") });
            Assert.IsFalse(EntityExtensions.AreEqual(c1, c2));

            c2.Entities.Clear();
            c2.Entities.Add(new Entity() { Id = Guid.Parse("{77E2CB34-A621-4CA9-844D-F484A0458D2F}") });
            c2.Entities.Add(new Entity() { Id = Guid.Parse("{489B9C29-73AD-4145-AEA0-E6CF03CB82DF}") });
            Assert.IsTrue(EntityExtensions.AreEqual(c1, c2));

            c1 = null;
            Assert.IsFalse(EntityExtensions.AreEqual(c1, c2));
        }

        [TestMethod()]
        public void IntsAreEqualTest()
        {
            //Setup
            int? i1 = null;
            int? i2 = null;

            //Act + Assert
            Assert.IsTrue(EntityExtensions.AreEqual(i1, i2));

            i1 = 1;
            Assert.IsFalse(EntityExtensions.AreEqual(i1, i2));

            i2 = 1;
            Assert.IsTrue(EntityExtensions.AreEqual(i1, i2));

            i2 = 2;
            Assert.IsFalse(EntityExtensions.AreEqual(i1, i2));

            i1 = null;
            Assert.IsFalse(EntityExtensions.AreEqual(i1, i2));
        }

        [TestMethod()]
        public void ShouldRemoveAllAttributesTest()
        {
            // Setup
            const string partyId = "{37825B8C-200B-4653-A270-927540B44435}";
            Guid guid = Guid.Parse("{FB105FC9-5050-41E2-96C4-E770E821E5FF}");
            const string referenceId = "{10AA9454-85C9-45CA-91E2-79DDD9B12D21}";
            DateTime dateTime = new DateTime(1985, 8, 8);
            const bool @bool = false;
            const int @int = 1;
            const long @long = 100000L;
            const double @double = 200.00;
            const decimal @decimal = 300.00m;
            const decimal money = 1000m;
            const int optionsetvalue = 0;

            EntityCollection tCollection = new EntityCollection();
            tCollection.Entities.Add(new Entity("activityparty", Guid.Parse(partyId)));

            var target = new Entity()
            {
                //Native types
                ["string"] = "Artem",
                ["guid"] = new Guid?(guid),
                ["datetime"] = new DateTime?(dateTime),
                ["bool"] = new bool?(@bool),
                ["int"] = new int?(@int),
                ["long"] = new long?(@long),
                ["double"] = new double?(@double),
                ["decimal"] = new decimal?(@decimal),
                //Dynamics types
                ["optionsetvalue"] = new OptionSetValue(optionsetvalue),
                ["entityreference"] = new EntityReference("team", Guid.Parse(referenceId)),
                ["money"] = new Money(money),
                ["entityimage"] = new byte[] { 1, 2, 3 },
                ["parylist"] = tCollection,
            };

            EntityCollection sCollection = new EntityCollection();
            sCollection.Entities.Add(new Entity("activityparty", Guid.Parse(partyId)));

            var source = new Entity()
            {
                LogicalName = "account",
                Id = guid,
                //Native types
                ["string"] = "Artem",
                ["guid"] = new Guid?(guid),
                ["datetime"] = new DateTime?(dateTime),
                ["bool"] = new bool?(@bool),
                ["int"] = new int?(@int),
                ["long"] = new long?(@long),
                ["double"] = new double?(@double),
                ["decimal"] = new decimal?(@decimal),
                //Dynamics types
                ["optionsetvalue"] = new OptionSetValue(optionsetvalue),
                ["entityreference"] = new EntityReference("team", Guid.Parse(referenceId)),
                ["money"] = new Money(money),
                ["entityimage"] = new byte[] { 1, 2, 3 },
                ["parylist"] = sCollection,
                ["inotexistintaget1"] = 123,
                ["inotexistintaget2"] = "hello",
                ["inotexistintaget3"] = new OptionSetValue(456)
            };

            // Act
            target.RemoveUnchanged(source);

            // Assert           
            Assert.AreEqual(0, target.Attributes.Count);
        }

        /// <summary>
        /// Just in case. Normally we shouldn't have nulls in source entity as
        /// system will not return attributes without values
        /// </summary>
        [TestMethod()]
        public void ShouldRemoveNothingComparingToNullsTest()
        {
            // Setup
            EntityCollection tCollection = new EntityCollection();
            tCollection.Entities.Add(new Entity("activityparty", Guid.NewGuid()));

            var target = new Entity()
            {
                //Native types
                ["string"] = "Artem",
                ["guid"] = new Guid?(Guid.NewGuid()),
                ["datetime"] = new DateTime?(new DateTime(1985, 8, 8)),
                ["bool"] = new bool?(false),
                ["int"] = new int?(1),
                ["long"] = new long?(100000L),
                ["double"] = new double?(200.00),
                ["decimal"] = new decimal?(300.00m),
                //Dynamics types
                ["optionsetvalue"] = new OptionSetValue(0),
                ["entityreference"] = new EntityReference("team", Guid.NewGuid()),
                ["money"] = new Money(1000m),
                ["entityimage"] = new byte[] { 1, 2, 3 },
                ["parylist"] = tCollection,
            };

            var source = new Entity()
            {
                LogicalName = "account",
                Id = Guid.NewGuid(),
                //Native types
                ["string"] = null,
                ["guid"] = null,
                ["datetime"] = null,
                ["bool"] = null,
                ["int"] = null,
                ["long"] = null,
                ["double"] = null,
                ["decimal"] = null,
                //Dynamics types
                ["optionsetvalue"] = null,
                ["entityreference"] = null,
                ["money"] = null,
                ["entityimage"] = null,
                ["parylist"] = null,
                ["inotexistintaget1"] = null,
                ["inotexistintaget2"] = null,
                ["inotexistintaget3"] = null
            };

            var expectedAttributeCount = target.Attributes.Count;

            // Act
            target.RemoveUnchanged(source);

            // Assert           
            Assert.AreEqual(expectedAttributeCount, target.Attributes.Count);
        }

        [TestMethod()]
        public void ShouldRemoveNothingTest()
        {
            // Setup
            EntityCollection tCollection = new EntityCollection();
            tCollection.Entities.Add(new Entity("activityparty", Guid.NewGuid()));

            var target = new Entity()
            {
                //Native types
                ["string"] = "Artem",
                ["guid"] = new Guid?(Guid.NewGuid()),
                ["datetime"] = new DateTime?(new DateTime(1985, 8, 8)),
                ["bool"] = new bool?(false),
                ["int"] = new int?(1),
                ["long"] = new long?(1L),
                ["double"] = new double?(200.00),
                ["decimal"] = new decimal?(300.00m),
                //Dynamics types
                ["optionsetvalue"] = new OptionSetValue(0),
                ["entityreference"] = new EntityReference("team", Guid.NewGuid()),
                ["money"] = new Money(1000m),
                ["entityimage"] = new byte[] { 1, 2, 3 },
                ["parylist"] = tCollection,
                ["inotexistinsource1"] = 1,
                ["inotexistinsource2"] = 2,
            };

            EntityCollection sCollection = new EntityCollection();
            sCollection.Entities.Add(new Entity("activityparty", Guid.NewGuid()));

            var source = new Entity()
            {
                LogicalName = "account",
                Id = Guid.NewGuid(),
                //Native types
                ["string"] = "Artem Grunin",
                ["guid"] = new Guid?(Guid.NewGuid()),
                ["datetime"] = DateTime.UtcNow,
                ["bool"] = new bool?(true),
                ["int"] = new int?(2),
                ["long"] = new long?(2L),
                ["double"] = new double?(300.00),
                ["decimal"] = new decimal?(400.00m),
                //Dynamics types
                ["optionsetvalue"] = new OptionSetValue(1),
                ["entityreference"] = new EntityReference("systemuser", Guid.NewGuid()),
                ["money"] = new Money(2000m),
                ["entityimage"] = new byte[] { 2, 3 },
                ["parylist"] = sCollection,
                ["inotexistintaget1"] = 1,
                ["inotexistintaget2"] = 2,
            };

            var expectedAttributeCount = target.Attributes.Count;

            // Act
            target.RemoveUnchanged(source);

            // Assert           
            Assert.AreEqual(expectedAttributeCount, target.Attributes.Count);
        }

        /// <summary>
        /// Just in case. Normally we shouldn't have nulls in source entity as
        /// system will not return attributes without values
        /// </summary>
        [TestMethod]
        public void ShouldRemoveNullsComparingToOtherNullsTest()
        {
            // Setup
            var target = new Entity()
            {
                //Native types
                ["string"] = null,
                ["guid"] = null,
                ["datetime"] = null,
                ["bool"] = null,
                ["int"] = null,
                ["long"] = null,
                ["double"] = null,
                ["decimal"] = null,
                //Dynamics types
                ["optionsetvalue"] = null,
                ["entityreference"] = null,
                ["money"] = null,
                ["entityimage"] = null,
                ["parylist"] = null,
            };

            var source = new Entity()
            {
                LogicalName = "account",
                Id = Guid.NewGuid(),
                //Native types
                ["string"] = null,
                ["guid"] = null,
                ["datetime"] = null,
                ["bool"] = null,
                ["int"] = null,
                ["long"] = null,
                ["double"] = null,
                ["decimal"] = null,
                //Dynamics types
                ["optionsetvalue"] = null,
                ["entityreference"] = null,
                ["money"] = null,
                ["entityimage"] = null,
                ["parylist"] = null,
                ["inotexistintaget1"] = null,
                ["inotexistintaget2"] = null,
                ["inotexistintaget3"] = null
            };

            // Act
            target.RemoveUnchanged(source);

            // Assert           
            Assert.AreEqual(0, target.Attributes.Count);
        }

        [TestMethod]
        public void ShouldRemoveNullsIfAttributeNotExistInSourceTest()
        {
            // Setup
            var target = new Entity()
            {
                //Native types
                ["whatever"] = null,
            };

            var source = new Entity()
            {
                LogicalName = "account",
                Id = Guid.NewGuid()
            };

            // Act
            target.RemoveUnchanged(source);

            // Assert           
            Assert.AreEqual(0, target.Attributes.Count);
        }

        [TestMethod()]
        public void CloneTest()
        {
            // Setup
            var entity = new Entity();
            entity.Id = Guid.NewGuid();
            entity.LogicalName = "account";
            entity.KeyAttributes["code"] = 123456;
            entity.Attributes["name"] = "FixRM";
            entity.FormattedValues["code"] = "123 456";

            // Act
            var clone = entity.Clone();

            // Assert
            Assert.AreNotEqual(entity, clone);

            Assert.AreEqual(clone.Id, entity.Id);
            Assert.AreEqual(clone.LogicalName, entity.LogicalName);
            CollectionAssert.AreEqual(clone.KeyAttributes.ToList(), entity.KeyAttributes.ToList());
            CollectionAssert.AreEqual(clone.Attributes.ToList(), entity.Attributes.ToList());
            CollectionAssert.AreEqual(clone.FormattedValues.ToList(), entity.FormattedValues.ToList());
        }

        [TestMethod()]
        public void TCloneTest()
        {
            // Setup
            var entity = new Account();
            entity.Id = Guid.NewGuid();
            entity.LogicalName = "account";
            entity.KeyAttributes["code"] = 123456;
            entity.Attributes["name"] = "FixRM";
            entity.FormattedValues["code"] = "123 456";

            // Act
            Account clone = entity.Clone();

            // Assert
            Assert.AreNotEqual(entity, clone);

            Assert.AreEqual(clone.Id, entity.Id);
            Assert.AreEqual(clone.LogicalName, entity.LogicalName);
            CollectionAssert.AreEqual(clone.KeyAttributes.ToList(), entity.KeyAttributes.ToList());
            CollectionAssert.AreEqual(clone.Attributes.ToList(), entity.Attributes.ToList());
            CollectionAssert.AreEqual(clone.FormattedValues.ToList(), entity.FormattedValues.ToList());
        }
    }
}
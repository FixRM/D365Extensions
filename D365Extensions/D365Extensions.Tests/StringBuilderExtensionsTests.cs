using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace D365Extensions.Tests
{
    [TestClass()]
    public class StringBuilderExtensionsTests
    {
        [TestMethod()]
        public void AppendEntityReferenceTest()
        {
            ///Setup
            const string id = "{9e0b39ed-1d15-443b-a312-a092f081d832}";
            const string logicalName = "account";
            string expectedTraceString = $@"EntityReference {{ LogicalName = ""{logicalName}"", Id = ""{id}"" }}";

            EntityReference entityReference = new EntityReference(logicalName, new Guid(id));


            ///Act
            var sb = new StringBuilder();
            sb.AppendObject(entityReference);

            var actualTraceString = sb.ToString();

            ///Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void AppendMoneyTest()
        {
            ///Setup
            const decimal value = 100.50M;
            string expectedTraceString = $"Money {{ Value = {value} }}";

            Money money = new Money(value);

            ///Act
            var sb = new StringBuilder();
            sb.AppendObject(money);

            string actualTraceString = sb.ToString();

            ///Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void AppendOptionSetTest()
        {
            ///Setup
            const int value = 1;
            var expectedTraceString = $"OptionSetValue {{ Value = {value} }}";

            OptionSetValue optionSet = new OptionSetValue(value);

            var sb = new StringBuilder();

            ///Act
            sb.AppendObject(optionSet);

            var actualTraceString = sb.ToString();

            ///Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void AppendEntityTest()
        {
            ///Setup
            const string id = "{9e0b39ed-1d15-443b-a312-a092f081d832}";
            const string logicalName = "account";
            Entity entity = new Entity(logicalName, new Guid(id));

            const string name = "name";
            const string value = "FixRM";
            entity.Attributes.Add(name, value);

            string expectedTraceString = 
$@"Entity {{ LogicalName = ""{logicalName}"", Id = ""{id}"" }}
Attributes: AttributeCollection {{ Count = 1 }}
""{name}"": ""{value}""";

            var sb = new StringBuilder();

            ///Act
            sb.AppendObject(entity);

            string actualTraceString = sb.ToString();

            ///Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void AppendDataCollectionTest()
        {
            ///Setup
            Entity entity = new Entity("account");
            OptionSetValue optionSet = new OptionSetValue(0);
            int rating = 5;

            const string target = "Target";
            const string stateCode = "StateCode";
            const string custom = "Custom";

            const string header = "InputParameters";

            ParameterCollection inputParameters = new ParameterCollection
            {
                { target, entity },
                { stateCode, optionSet },
                { custom, rating }
            };

            string expectedTraceString =
$@"{header}: ParameterCollection {{ Count = 3 }}
""{target}"": Entity {{ LogicalName = ""account"", Id = ""{{00000000-0000-0000-0000-000000000000}}"" }}
Attributes: AttributeCollection {{ Count = 0 }}
""{stateCode}"": OptionSetValue {{ Value = 0 }}
""{custom}"": {rating}";

            var sb = new StringBuilder();

            ///Act
            sb.AppendDataCollection(inputParameters, header);

            string actualTraceString = sb.ToString();

            ///Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void AppendEntityCollectionTest()
        {
            ///Setup
            Entity party1 = new Entity("contact");
            party1["email"] = "artem@grunin.ru";
            Entity party2 = new Entity("account");
            party2["email"] = "fix@rm.ru";

            const string activityparty = "activityparty";
            EntityCollection collection = new EntityCollection();
            collection.EntityName = activityparty;

            collection.Entities.Add(party1);
            collection.Entities.Add(party2);

            Entity entity = new Entity("email");
            entity["to"] = collection;

            string expectedTraceString = 
$@"Entity {{ LogicalName = ""email"", Id = ""{{00000000-0000-0000-0000-000000000000}}"" }}
Attributes: AttributeCollection {{ Count = 1 }}
""to"": EntityCollection {{ EntityName = ""{activityparty}"" }}
Entities: {{ Count = 2 }} 
Entity {{ LogicalName = ""contact"", Id = ""{{00000000-0000-0000-0000-000000000000}}"" }}
Attributes: AttributeCollection {{ Count = 1 }}
""email"": ""artem@grunin.ru""
Entity {{ LogicalName = ""account"", Id = ""{{00000000-0000-0000-0000-000000000000}}"" }}
Attributes: AttributeCollection {{ Count = 1 }}
""email"": ""fix@rm.ru""";

            var sb = new StringBuilder();

            ///Act
            sb.AppendObject(entity);

            string actualTraceString = sb.ToString();

            ///Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void AppendStringTest()
        {
            ///Setup
            const string str = "key";            

            string expectedTraceString = $@"""{str}""";

            var sb = new StringBuilder();

            ///Act
            sb.AppendObject(str);

            string actualTraceString = sb.ToString();

            ///Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }

        [TestMethod()]
        public void AppendQuidTest()
        {
            ///Setup
            Guid id = Guid.Empty;

            string expectedTraceString = $@"""{{00000000-0000-0000-0000-000000000000}}""";

            var sb = new StringBuilder();

            ///Act
            sb.AppendObject(id);

            string actualTraceString = sb.ToString();

            ///Assert
            Assert.AreEqual(expectedTraceString, actualTraceString);
        }
    }
}
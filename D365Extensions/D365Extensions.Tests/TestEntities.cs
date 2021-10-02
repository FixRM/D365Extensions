using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace D365Extensions.Tests
{
    /// <summary>
    /// Simple Entity inheritor for Test purpose
    /// </summary>
    [EntityLogicalName("testentity")]
    public class TestEntity : Entity
    {
        public const string EntityLogicalName = "testentity";

        public TestEntity() : base(EntityLogicalName)
        {
        }

        /// <summary>
        /// Reference type property
        /// </summary>
        [AttributeLogicalName("referencetypeproperty")]
        public string ReferenceTypeProperty
        {
            get
            {
                return this.GetAttributeValue<string>("referencetypeproperty");
            }
            set
            {
                this.SetAttributeValue("referencetypeproperty", value);
            }
        }

        /// <summary>
        /// Value type property
        /// </summary>
        [AttributeLogicalName("valuetypeproperty")]
        public System.Nullable<int> ValueTypeProperty
        {
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>("valuetypeproperty");
            }
            set
            {
                this.SetAttributeValue("valuetypeproperty", value);
            }
        }
    }

    /// <summary>
    /// Simple Entity for LinkEntity tests
    /// </summary>
    [EntityLogicalName("entityfrom")]
    public class EntityFrom : Entity
    {
        public static string EnityLogicalName = "entityfrom";

        public EntityFrom() : base(EnityLogicalName)
        {
        }

        [AttributeLogicalName("fromid")]
        public Guid FromId { get; set; }
    }

    /// <summary>
    /// Simple Entity for LinkEntity tests
    /// </summary>
    [EntityLogicalName("entityto")]
    public class EntityTo : Entity
    {
        public static string EnityLogicalName = "entityto";

        public EntityTo() : base(EnityLogicalName)
        {
        }

        [AttributeLogicalName("toid")]
        public Guid ToId { get; set; }
    }

    /// <summary>
    /// Entity for UseReflection tests
    /// </summary>
    [EntityLogicalName("custom_entity")]
    public class CustomEntity : Entity
    {
        public static string EnityLogicalName = "custom_entity";

        public CustomEntity() : base(EnityLogicalName)
        {
        }

        [AttributeLogicalName("string_prop")]
        public string StringProp {
            get => GetAttributeValue<string>("string_prop");
            set => SetAttributeValue("string_prop", value); 
        }
    }

    /// <summary>
    /// Entity for UseReflection tests
    /// </summary>
    [EntityLogicalName("custom_entity2")]
    public class CustomEntity2 : Entity
    {
        public static string EnityLogicalName = "custom_entity2";

        public CustomEntity2() : base(EnityLogicalName)
        {
        }

        [AttributeLogicalName("string_prop2")]
        public string StringProp
        {
            get => GetAttributeValue<string>("string_prop2");
            set => SetAttributeValue("string_prop2", value);
        }
    }
}

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
    /// Entity for bencmark
    /// </summary>
    [EntityLogicalName("custom_entity")]
    public class CustomEntity : Entity
    {
        public static string EnityLogicalName = "custom_entity";

        public CustomEntity() : base(EnityLogicalName)
        {
        }

        [AttributeLogicalName("prop_1")]
        public string Property_1
        {
            get => GetAttributeValue<string>("prop_1");
            set => SetAttributeValue("prop_1", value); 
        }

        [AttributeLogicalName("prop_2")]
        public int? Property_2
        {
            get => GetAttributeValue<int?> ("prop_2");
            set => SetAttributeValue("prop_2", value);
        }
    }
}

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
    [EntityLogicalName("fixrm_testentity")]
    public class TestEntity : Entity
    {
        public const string EntityLogicalName = "fixrm_testentity";

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
}

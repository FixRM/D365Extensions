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
    [EntityLogicalNameAttribute("fixrm_testentity")]
    public class TestEntity : Entity
    {
        public TestEntity() : base("fixrm_testentity")
        {

        }
    }
}

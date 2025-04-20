using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Strongly typed version of the KeyAttributeCollection class
    /// </summary>
    public sealed class KeyAttributeCollection<T> : DataCollection<Expression<Func<T,object>>, object> where T : Entity
    {
        public static implicit operator KeyAttributeCollection(KeyAttributeCollection<T> t)
        {
            if (t == null) return null;

            var keys = new KeyAttributeCollection();
            
            foreach (var kv in t)
            {
                keys.Add(LogicalName.GetName(kv.Key), kv.Value);
            }

            return keys;
        }
    }
}

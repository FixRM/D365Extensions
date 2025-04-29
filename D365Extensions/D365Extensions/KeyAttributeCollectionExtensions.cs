using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk
{
    public static class KeyAttributeCollectionExtensions
    {
        /// <summary>
        /// Adds the specified key and value to the dictionary
        /// </summary>
        /// <typeparam name="T">Type of the entity to add column from</typeparam>
        /// <param name="key">The property expressions containing the name of the attribute to add</param>
        /// <param name="value">Attribute value</param>
        public static void Add<T>(this KeyAttributeCollection collection, Expression<Func<T, object>> key, object value) where T : Entity
        {
            collection.Add(LogicalName.GetName(key), value);
        }
    }
}
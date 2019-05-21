using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Set of extension methods for Microsoft.Xrm.Sdk.EntityReference base class. At the moment just two simple but sometimes useful type conversion methods.
    /// </summary>
    public static class EntityReferenceExtensions
    {
        /// <summary>
        /// Gets the entity based on the EntityReference.
        /// </summary>
        /// <returns></returns>
        public static Entity ToEntity(this EntityReference entityReference)
        {
            return new Entity()
            {
                Id = entityReference.Id,
                LogicalName = entityReference.LogicalName,
                KeyAttributes = entityReference.KeyAttributes,
                RowVersion = entityReference.RowVersion
            };
        }

        /// <summary>
        /// Gets the entity based on the EntityReference as the specified type.
        /// </summary>
        /// <returns></returns>
        public static T ToEntity<T>(this EntityReference entityReference) where T : Entity
        {
            return entityReference.ToEntity().ToEntity<T>();
        }
    }
}

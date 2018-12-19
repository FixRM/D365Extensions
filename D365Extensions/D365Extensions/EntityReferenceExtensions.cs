using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk
{
    public static class EntityReferenceExtensions
    {
        /// <summary>
        /// Gets the entity based on the EntityReference
        /// </summary>
        /// <returns></returns>
        public static Entity ToEntity(this EntityReference entityReference)
        {
            return new Entity()
            {
                Id = entityReference.Id,
                LogicalName = entityReference.LogicalName,
            };
        }

        /// <summary>
        /// Gets the entity based on the EntityReference as the specified type
        /// </summary>
        /// <returns></returns>
        public static T ToEntity<T>(this EntityReference entityReference) where T : Entity
        {
            return entityReference.ToEntity().ToEntity<T>();
        }
    }
}

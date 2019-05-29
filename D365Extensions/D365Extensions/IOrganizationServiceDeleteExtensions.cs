using D365Extensions;
using Microsoft.Xrm.Sdk.Messages;
using System;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Extensions for IOrganizationService.Delete
    /// </summary>
    public static partial class IOrganizationServiceExtensions
    {
        /// <summary>
        /// Delete method override. Takes EntityReference as input parameter
        /// </summary>
        /// <param name="reference">Entity to delete</param>
        public static void Delete(this IOrganizationService service, EntityReference reference)
        {
            CheckParam.CheckForNull(reference, nameof(reference));

            /// Delete by id if possible as is supposed to be faster 
            if (reference.Id != Guid.Empty)
            {
                service.Delete(reference.LogicalName, reference.Id);
            }
            /// Use alternative key
            else
            {
                service.Delete(reference.LogicalName, reference.KeyAttributes);
            }

        }

        /// <summary>
        /// Delete method override. Takes Entity as input parameter
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        public static void Delete(this IOrganizationService service, Entity entity)
        {
            CheckParam.CheckForNull(entity, nameof(entity));

            service.Delete(entity.ToEntityReference(true));
        }

        /// <summary>
        /// Delete method override. Deletes by Alternative key
        /// </summary>
        /// <param name="reference">Entity to delete</param>
        public static void Delete(this IOrganizationService service, string logicalName, KeyAttributeCollection keyAttributeCollection)
        {
            CheckParam.CheckForNull(logicalName, nameof(logicalName));
            CheckParam.CheckForNull(keyAttributeCollection, nameof(keyAttributeCollection));

            service.Execute(new DeleteRequest()
            {
                Target = new EntityReference(logicalName, keyAttributeCollection)
            });
        }

        /// <summary>
        /// Delete method override. Deletes by Alternative key
        /// </summary>
        /// <param name="reference">Entity to delete</param>
        public static void Delete(this IOrganizationService service, string logicalName, string keyName, object keyValue)
        {
            CheckParam.CheckForNull(keyName, nameof(keyName));
            CheckParam.CheckForNull(keyValue, nameof(keyValue));

            KeyAttributeCollection keys = new KeyAttributeCollection();
            keys.Add(keyName, keyValue);

            service.Delete(logicalName, keys);
        }
    }
}

using D365Extensions;
using System;
using System.Collections.Generic;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Extensions for IOrganizationService.Associate
    /// </summary>
    public static partial class IOrganizationServiceExtensions
    {
        /// <summary>
        /// Associate method override. Takes EntityReference as primary entity input parameter
        /// </summary>        
        public static void Associate(this IOrganizationService service, EntityReference primaryEntity, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            CheckParam.CheckForNull(primaryEntity, nameof(primaryEntity));

            service.Associate(primaryEntity.LogicalName, primaryEntity.Id, relationship, relatedEntities);
        }

        /// <summary>
        /// Associate method override. Takes EntityReference as primary entity input parameter and list of EntityReferences as related entities parameter
        /// </summary>
        public static void Associate(this IOrganizationService service, EntityReference primaryEntity, Relationship relationship, IList<EntityReference> relatedEntities)
        {
            service.Associate(primaryEntity, relationship, new EntityReferenceCollection(relatedEntities));
        }

        /// <summary>
        /// Associate method override. Takes EntityReference as primary entity input parameter
        /// </summary>        
        public static void Associate(this IOrganizationService service, EntityReference primaryEntity, String relationshipName, EntityReferenceCollection relatedEntities)
        {
            service.Associate(primaryEntity.LogicalName, primaryEntity.Id, new Relationship(relationshipName), relatedEntities);
        }

        /// <summary>
        /// Associate method override. Takes EntityReference as primary entity input parameter and list of EntityReferences as related entities parameter
        /// </summary>
        public static void Associate(this IOrganizationService service, EntityReference primaryEntity, String relationshipName, IList<EntityReference> relatedEntities)
        {
            service.Associate(primaryEntity, relationshipName, new EntityReferenceCollection(relatedEntities));
        }
    }
}

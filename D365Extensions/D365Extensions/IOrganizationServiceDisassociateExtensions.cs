using D365Extensions;
using System;
using System.Collections.Generic;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Extensions for IOrganizationService.Disassociate
    /// </summary>
    public static partial class IOrganizationServiceExtensions
    {
        /// <summary>
        /// Disassociate method override. Takes EntityReference as primary entity input parameter
        /// </summary>
        public static void Disassociate(this IOrganizationService service, EntityReference primaryEntity, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            CheckParam.CheckForNull(primaryEntity, nameof(primaryEntity));

            service.Disassociate(primaryEntity.LogicalName, primaryEntity.Id, relationship, relatedEntities);
        }

        /// <summary>
        /// Disassociate method override. Takes EntityReference as primary entity input parameter and list of EntityReferences as related entities parameter
        /// </summary>
        public static void Disassociate(this IOrganizationService service, EntityReference primaryEntity, Relationship relationship, IList<EntityReference> relatedEntities)
        {

            service.Disassociate(primaryEntity, relationship, new EntityReferenceCollection(relatedEntities));
        }

        /// <summary>
        /// Associate method override. Takes EntityReference as primary entity input parameter
        /// </summary>        
        public static void Disassociate(this IOrganizationService service, EntityReference primaryEntity, String relationshipName, EntityReferenceCollection relatedEntities)
        {
            service.Disassociate(primaryEntity.LogicalName, primaryEntity.Id, new Relationship(relationshipName), relatedEntities);
        }

        /// <summary>
        /// Associate method override. Takes EntityReference as primary entity input parameter and list of EntityReferences as related entities parameter
        /// </summary>
        public static void Disassociate(this IOrganizationService service, EntityReference primaryEntity, String relationshipName, IList<EntityReference> relatedEntities)
        {
            service.Disassociate(primaryEntity, relationshipName, new EntityReferenceCollection(relatedEntities));
        }
    }
}

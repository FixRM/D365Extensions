using D365Extensions;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Extensions for IOrganizationService
    /// </summary>
    public static class IOrganizationServiceExtensions
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

        /// <summary>
        /// Retrieve method override. Takes EntityReference as input parameter
        /// </summary>
        /// <param name="reference">Entity to retrieve</param>
        public static Entity Retrieve(this IOrganizationService service, EntityReference reference, ColumnSet columnSet)
        {
            CheckParam.CheckForNull(reference, nameof(reference));

            /// Retrieve by id if possible as is supposed to be faster
            if (reference.Id != Guid.Empty)
            {
                return service.Retrieve(reference.LogicalName, reference.Id, columnSet);
            }
            /// Use alternative key
            else
            {
                return service.Retrieve(reference.LogicalName, reference.KeyAttributes, columnSet);
            }
        }

        /// <summary>
        /// Retrieve method override. Takes EntityReference as input parameter and array of attribute names to retrieve
        /// </summary>
        /// <param name="reference">Entity to retrieve</param>
        public static Entity Retrieve(this IOrganizationService service, EntityReference reference, params String[] columns)
        {
            return service.Retrieve(reference, new ColumnSet(columns));
        }

        /// <summary>
        /// Retrieve method override. Takes EntityReference as input parameter and return strongly typed entity object
        /// </summary>
        /// <param name="reference">Entity to retrieve</param>
        public static T Retrieve<T>(this IOrganizationService service, EntityReference reference, ColumnSet columnSet) where T : Entity
        {
            Entity entity = service.Retrieve(reference, columnSet);

            return entity.ToEntity<T>();
        }

        /// <summary>
        /// Retrieve method override. Takes EntityReference as input parameter and return strongly typed entity object
        /// </summary>
        /// <param name="reference">Entity to retrieve</param>
        public static T Retrieve<T>(this IOrganizationService service, EntityReference reference, params String[] columns) where T : Entity
        {
            return service.Retrieve<T>(reference, new ColumnSet(columns));
        }

        /// <summary>
        /// Retrieve method override. Retrieves by Alternative key
        /// </summary>
        /// <param name="keyName">Name of alternative key</param>
        /// <param name="keyValue">Key value</param>
        public static Entity Retrieve(this IOrganizationService service, string logicalName, KeyAttributeCollection keyAttributeCollection, ColumnSet columnSet)
        {
            CheckParam.CheckForNull(logicalName, nameof(logicalName));
            CheckParam.CheckForNull(keyAttributeCollection, nameof(keyAttributeCollection));

            RetrieveResponse response = service.Execute(new RetrieveRequest()
            {
                Target = new EntityReference(logicalName, keyAttributeCollection),
                ColumnSet = columnSet
            }) as RetrieveResponse;

            return response.Entity;
        }

        /// <summary>
        /// Retrieve method override. Retrieves by Alternative key
        /// </summary>
        /// <param name="keyName">Name of alternative key</param>
        /// <param name="keyValue">Key value</param>
        public static Entity Retrieve(this IOrganizationService service, string logicalName, string keyName, object keyValue, ColumnSet columnSet)
        {
            CheckParam.CheckForNull(keyName, nameof(keyName));
            CheckParam.CheckForNull(keyValue, nameof(keyValue));

            KeyAttributeCollection keys = new KeyAttributeCollection();
            keys.Add(keyName, keyValue);

            return service.Retrieve(logicalName, keys, columnSet);
        }

        /// <summary>
        /// Retrieve method override. Retrieves by Alternative key 
        /// </summary>
        /// <param name="keyName">Name of alternative key</param>
        /// <param name="keyValue">Key value</param>
        public static Entity Retrieve(this IOrganizationService service, string logicalName, string keyName, object keyValue, params string[] columns)
        {
            return service.Retrieve(logicalName, keyName, keyValue, new ColumnSet(columns));
        }

        /// <summary>
        /// Retrieve method override. Retrieves by Alternative key and returns strongly typed entity object
        /// </summary>
        /// <param name="keyName">Name of alternative key</param>
        /// <param name="keyValue">Key value</param>
        public static T Retrieve<T>(this IOrganizationService service, string logicalName, string keyName, string keyValue, ColumnSet columnSet) where T : Entity
        {
            Entity entity = service.Retrieve(logicalName, keyName, keyValue, columnSet);

            return entity.ToEntity<T>();
        }

        /// <summary>
        /// Retrieve method override. Retrieves by Alternative key and returns strongly typed entity object
        /// </summary>
        /// <param name="keyName">Name of alternative key</param>
        /// <param name="keyValue">Key value</param>
        public static T Retrieve<T>(this IOrganizationService service, string logicalName, string keyName, string keyValue, params String[] columns) where T : Entity
        {
            return service.Retrieve<T>(logicalName, keyName, keyValue, new ColumnSet(columns));
        }
    }
}
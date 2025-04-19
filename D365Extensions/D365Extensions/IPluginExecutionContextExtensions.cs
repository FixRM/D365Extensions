using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Extensions for IPluginExecutionContext
    /// </summary>
    public static class IPluginExecutionContextExtensions
    {
        private const string Target = "Target";
        private const string PreImage = "PreImage";
        private const string PostImage = "PostImage";

        /// <summary>
        /// Return OrganizationId and OrganizationName fields as single EntityReference
        /// </summary>
        /// <returns></returns>
        public static EntityReference GetOrganization(this IPluginExecutionContext context)
        {
            return new EntityReference()
            {
                Id = context.OrganizationId,
                Name = context.OrganizationName,
                LogicalName = "organization"
            };
        }

        /// <summary>
        /// Return PrimaryEntityId and PrimaryEntityName fields as single EntityReference
        /// </summary>
        /// <returns></returns>
        public static EntityReference GetPrimaryEntity(this IPluginExecutionContext context)
        {
            if (Guid.Empty.Equals(context.PrimaryEntityId))
            {
                return null;
            }

            return new EntityReference()
            {
                Id = context.PrimaryEntityId,
                LogicalName = context.PrimaryEntityName
            };
        }

        /// <summary>
        /// Return UserId field as EntityReference
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static EntityReference GetUser(this IPluginExecutionContext context)
        {
            return new EntityReference()
            {
                Id = context.UserId,
                LogicalName = "systemuser"
            };
        }

        /// <summary>
        /// Return InitiatingUserId field as EntityReference
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static EntityReference GetInitiatingUser(this IPluginExecutionContext context)
        {
            return new EntityReference()
            {
                Id = context.InitiatingUserId,
                LogicalName = "systemuser"
            };
        }

        /// <summary>
        /// Return BusinessUnitId field as EntityReference
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static EntityReference GetBusinessUnit(this IPluginExecutionContext context)
        {
            return new EntityReference()
            {
                Id = context.BusinessUnitId,
                LogicalName = "businessunit"
            };
        }

        /// <summary>
        /// Gets input parameter
        /// 
        /// This method is not safe. Use context.InputParameters.GetValue<T>() for optional parameters
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns></returns>
        /// <exception cref="
        public static T GetInputParameter<T>(this IPluginExecutionContext context, String name)
        {
            return (T)context.InputParameters[name];
        }

        /// <summary>
        /// Gets output parameter
        /// 
        /// /// This method is not safe. Use context.OutputParameters.GetValue<T>() for optional parameters
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns></returns>
        public static T GetOutputParameter<T>(this IPluginExecutionContext context, String name)
        {
            return (T)context.OutputParameters[name];
        }

        /// <summary>
        /// Gets pre image
        /// </summary>
        /// <param name="name">Image name</param>
        /// <returns></returns>
        public static Entity GetPreImage(this IPluginExecutionContext context, string name = PreImage)
        {
            return context.PreEntityImages[name];
        }

        /// <summary>
        /// Gets pre image as the specified type
        /// </summary>
        /// <param name="name">Image name</param>
        /// <returns></returns>
        public static T GetPreImage<T>(this IPluginExecutionContext context, string name = PreImage) where T : Entity
        {
            return context.PreEntityImages[name]?.ToEntity<T>();
        }

        /// <summary>
        /// Gets post image
        /// </summary>
        /// <param name="name">Image name</param>
        /// <returns></returns>
        public static Entity GetPostImage(this IPluginExecutionContext context, string name = PostImage)
        {
            return context.PostEntityImages[name];
        }

        /// <summary>
        /// Gets post image as the specified type
        /// </summary>
        /// <param name="name">Image name</param>
        /// <returns></returns>
        public static T GetPostImage<T>(this IPluginExecutionContext context, string name = PostImage) where T : Entity
        {
            return context.PostEntityImages[name]?.ToEntity<T>();
        }

        /// <summary>
        /// Shortcut for getting "Target" input parameter of type Entity
        /// </summary>
        /// <returns></returns>
        public static Entity GetTarget(this IPluginExecutionContext context)
        {
            return GetInputParameter<Entity>(context, Target);
        }

        /// <summary>
        /// Shortcut for getting "Target" input parameter  as the specified type
        /// </summary>
        /// <returns></returns>
        public static T GetTarget<T>(this IPluginExecutionContext context) where T : Entity
        {
            return GetTarget(context)?.ToEntity<T>();
        }

        /// <summary>
        /// Get "Target" entity parameter merged with specified pre image 
        /// </summary>
        /// <param name="preImageName">Image name</param>
        /// <returns></returns>
        public static T GetPreTarget<T>(this IPluginExecutionContext context, string preImageName = PreImage) where T : Entity
        {
            return GetPreTarget(context, preImageName).ToEntity<T>();
        }

        /// <summary>
        /// Get "Target" entity parameter merged with specified pre image 
        /// </summary>
        /// <param name="preImageName">Image name</param>
        /// <returns></returns>
        public static Entity GetPreTarget(this IPluginExecutionContext context, string preImageName = PreImage)
        {
            Entity target = GetTarget(context);
            Entity image = GetPreImage(context, preImageName);

            Entity result = target.Clone();
            result.MergeAttributes(image);

            return result;
        }

        /// <summary>
        /// Get "Target" entity parameter merged with specified post image 
        /// </summary>
        /// <param name="postImageName">Image name</param>
        /// <returns></returns>
        public static T GetPostTarget<T>(this IPluginExecutionContext context, string postImageName = PostImage) where T : Entity
        {
            //TODO: use generic
            return GetPostTarget(context, postImageName).ToEntity<T>();
        }

        /// <summary>
        /// Get "Target" entity parameter merged with specified post image 
        /// </summary>
        /// <param name="postImageName">Image name</param>
        /// <returns></returns>
        public static Entity GetPostTarget(this IPluginExecutionContext context, string postImageName = PostImage)
        {
            Entity target = GetTarget(context);
            Entity image = GetPostImage(context, postImageName);

            Entity result = target.Clone();
            result.MergeAttributes(image);

            return result;
        }

        /// <summary>
        /// Gets shared Variable
        /// 
        /// This method is not safe. Use context.SharedVariables.GetValue<string>() for optional parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetSharedVariable<T>(this IPluginExecutionContext context, string name)
        {
            return (T)context.SharedVariables[name];
        }

        /// <summary>
        /// Simplifies handling of Associate and Disassociate messages. This messages can't be filtered by entity type. Furthermore
        /// two options possible: when "A" entity is associated with array of "B", or "B" is associated with array of "A".
        /// 
        /// This method generates universal dictionary of arguments which is suitable in all cases
        /// </summary>
        /// <param name="keyEntity">Key entity schema name</param>
        /// <param name="valueEntity">Secondary entity schema name</param>
        /// <returns></returns>
        [Obsolete("Use GetRelatedEntitiesAsTuples")]
        public static Dictionary<EntityReference, EntityReferenceCollection> GetRelatedEntitiesByTarget(this IPluginExecutionContext pluginContext,
            String keyEntity,
            String valueEntity)
        {
            /// Check that we handling appropriate message
            if (pluginContext.MessageName != "Associate" && pluginContext.MessageName != "Disassociate")
            {
                throw new InvalidOperationException($"This method is not supported for {pluginContext.MessageName} message");
            }

            /// Get InputParameters for Associate и Disassociate 
            EntityReference target = pluginContext.InputParameters[Target] as EntityReference;
            EntityReferenceCollection relatedEntities = pluginContext.InputParameters["RelatedEntities"] as EntityReferenceCollection;

            /// Get schema names for this participating entities
            String targetName = target.LogicalName;
            String relatedName = relatedEntities.First().LogicalName;

            /// Generate result dictionary
            Dictionary<EntityReference, EntityReferenceCollection> dictionary = new Dictionary<EntityReference, EntityReferenceCollection>(relatedEntities.Count);

            if (targetName == keyEntity && relatedName == valueEntity)
            {
                dictionary.Add(target, relatedEntities);
            }
            else if (relatedName == keyEntity && targetName == valueEntity)
            {
                foreach (EntityReference key in relatedEntities)
                {
                    dictionary.Add(key, new EntityReferenceCollection() { target });
                }
            }

            return dictionary;
        }

        internal const string WrongMessageForGetRelatedEntities = "This method is not supported for {0} message";

        /// <summary>
        /// Simplifies handling of Associate and Disassociate messages. This messages can't be filtered by entity type. Furthermore
        /// two options possible: when "A" entity is associated with array of "B", or "B" is associated with array of "A".
        /// 
        /// This method generates universal pair of arguments which is suitable in all cases
        /// </summary>
        /// <param name="targetEntityLogicalName">Key entity schema name</param>
        /// <returns>List of Tuples or empty list if associated/disassociated entity types don't match with required</returns>
        public static IEnumerable<Tuple<EntityReference, EntityReference>> GetRelatedEntitiesAsTuples(this IPluginExecutionContext pluginContext,
            string targetEntityLogicalName,
            string relatedEntityLogicalName)
        {
            /// Check that we handling appropriate message
            if (pluginContext.MessageName != "Associate" && pluginContext.MessageName != "Disassociate")
            {
                throw new InvalidOperationException(string.Format(WrongMessageForGetRelatedEntities, pluginContext.MessageName));
            }

            /// Get InputParameters for Associate и Disassociate 
            EntityReference target = pluginContext.InputParameters[Target] as EntityReference;
            EntityReferenceCollection relatedEntities = pluginContext.InputParameters["RelatedEntities"] as EntityReferenceCollection;

            /// Get schema names for this participating entities
            string targetName = target.LogicalName;
            string relatedName = relatedEntities.First().LogicalName;

            foreach (EntityReference relatedReference in relatedEntities)
            {
                if (targetName == targetEntityLogicalName && relatedName == relatedEntityLogicalName)
                {
                    yield return new Tuple<EntityReference, EntityReference>(target, relatedReference);
                }
                else if (relatedName == targetEntityLogicalName && targetName == relatedEntityLogicalName)
                {
                    yield return new Tuple<EntityReference, EntityReference>(relatedReference, target);
                }
                else break;
            }

            yield break;
        }
    }
}

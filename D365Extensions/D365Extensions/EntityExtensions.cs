using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Xrm.Sdk
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Simplifies getting values from Entity.FormattedValues collection
        /// <param name="attributeLogicalName">Attribute name</param>
        /// <returns>Attribute formated value</returns>
        public static String GetFormatedValue(this Entity entity, String attributeLogicalName)
        {
            CheckParam.CheckForNull(attributeLogicalName, nameof(attributeLogicalName));

            entity.FormattedValues.TryGetValue(attributeLogicalName, out string outValue);
            return outValue;
        }

        /// <summary>
        /// Simplifies getting values from linked entities attributes wrapped in AliasedValue class
        /// This kind of attributes can be queried by FetchExpression or QueryExpression using Linked Entities 
        /// </summary>
        /// <typeparam name="T">Attribute value type</typeparam>
        /// <param name="attributeLogicalName">Attribute logical name</param>
        /// <param name="alias">>Entity alias used in LinkedEntity definition</param>
        /// <returns>Attribute value</returns>
        public static T GetAliasedValue<T>(this Entity entity, String attributeLogicalName, String alias)
        {
            CheckParam.CheckForNull(attributeLogicalName, nameof(attributeLogicalName));
            CheckParam.CheckForNull(alias, nameof(alias));

            String aliasedAttributeName = alias + "." + attributeLogicalName;
            AliasedValue aliasedValue = entity.GetAttributeValue<AliasedValue>(aliasedAttributeName);

            if (aliasedValue != null)
            {
                return (T)aliasedValue.Value;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Simplifies getting multiple linked entity attributes by allocating them to separate Entity
        /// </summary>
        /// <param name="entityLogicalName">Logical name of linked Entity</param>
        /// <param name="alias">Entity alias used in LinkedEntity definition</param>
        /// <returns>Entity with specified logical name that contains all attribute values with specified alias</returns>
        public static Entity GetAliasedEntity(this Entity entity, String entityLogicalName, String alias = null)
        {
            /// Use LogicalName as alias if it is not specified
            String aliasPrefix = alias ?? entityLogicalName + ".";

            var aliasedAttributes = entity.Attributes.Where(a => a.Key.StartsWith(aliasPrefix))
                .Select(a => a.Value as AliasedValue)
                .Where(a => a != null)
                .Select(a => new KeyValuePair<String, Object>(a.AttributeLogicalName, a.Value));

            Entity aliasedEntity = new Entity(entityLogicalName);
            aliasedEntity.Attributes.AddRange(aliasedAttributes);

            return aliasedEntity;
        }

        /// <summary>
        /// Generic version of GetAliasedEntity
        /// </summary>
        /// <param name="entityLogicalName">Logical name of linked Entity</param>
        /// <param name="alias">Entity alias used in LinkedEntity definition</param>
        /// <returns></returns>
        [Obsolete("Use public T GetAliasedEntity<T>(String alias = null) where T : Entity; method instead")]
        public static T GetAliasedEntity<T>(this Entity entity, String entityLogicalName, String alias = null) where T : Entity
        {
            return GetAliasedEntity(entity, entityLogicalName, alias).ToEntity<T>();
        }

        /// <summary>
        /// Generic version of GetAliasedEntity
        /// </summary>
        /// <param name="alias">Entity alias used in LinkedEntity definition</param>
        /// <returns></returns>
        public static T GetAliasedEntity<T>(this Entity entity, String alias = null) where T : Entity
        {
            return GetAliasedEntity(entity, LogicalName.GetName<T>(), alias).ToEntity<T>();
        }

        /// <summary>
        /// Add attributes form source Entity if they don't exist in target Entity
        /// </summary>
        /// <param name="source">Entity to take attributes form </param>
        public static void MergeAttributes(this Entity target, Entity source)
        {
            if (source != null)
            {
                foreach (var attribute in source.Attributes)
                {
                    if (target.Attributes.ContainsKey(attribute.Key) == false)
                    {
                        target.Attributes.Add(attribute);
                    }
                }
            }
        }

        /// <summary>
        /// Safely sets attribute value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true if new attribute added</returns>
        public static bool SetAttributeValue(this Entity entity, String name, Object value)
        {
            if (entity.Contains(name))
            {
                entity[name] = value;
                return false;
            }
            else
            {
                entity.Attributes.Add(name, value);
                return true;
            }
        }

        /// <summary>
        /// As it turns out, OOB ToEntityReference is not copying KeyAttributes collection
        /// New parameter has to be added to be valid override of ToEntityReference
        /// </summary>
        /// <param name="withKeys">Copy KeyAttributes collection</param>
        /// <returns></returns>
        public static EntityReference ToEntityReference(this Entity entity, bool withKeys)
        {
            EntityReference reference = entity.ToEntityReference();

            if (withKeys == true)
            {
                reference.KeyAttributes = entity.KeyAttributes;
            }
            
            return reference;
        }
    }
}

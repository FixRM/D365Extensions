using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <param name="attributeLogicalName">Optional. Attribute logical name</param>
        /// <param name="alias">>Entity alias used in LinkedEntity definition</param>
        /// <returns>Attribute value</returns>
        public static T GetAliasedValue<T>(this Entity entity, String attributeLogicalName, String alias)
        {
            CheckParam.CheckForNull(alias, nameof(alias));

            String aliasedAttributeName = attributeLogicalName != null ? alias + "." + attributeLogicalName : alias;
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
            CheckParam.CheckForNull(entityLogicalName, nameof(entityLogicalName));

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
        [Obsolete("blame on me, use entity[] to set values")]
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

        /// <summary>
        /// Returns Entity string representation
        /// </summary>
        /// <returns></returns>
        public static string ToTraceString(this Entity entity)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendEntity(entity);

            return sb.ToString();
        }

        public static Entity Changes(this Entity target, Entity source)
        {
            return target.Changes<Entity>(source);
        }

        public static T Changes<T>(this T target, T source) where T : Entity, new()
        {
            return target.Changes(source, new EntityDeconstructor<T>());
        }

        public static TTarget Changes<TTarget, TSource>(this TTarget target, TSource source) where TTarget : Entity, new()
        {
            return target.Changes(source, new EntityReflectionDeconstructor<TSource>());
        }

        // TODO: should we check if ID and LogicalName are equal?
        public static TTarget Changes<TTarget, TSource>(this TTarget target, TSource source, IDeconstructor<TSource> deconstructor) where TTarget : Entity, new()
        {
            CheckParam.CheckForNull(source, nameof(source));
            CheckParam.CheckForNull(deconstructor, nameof(deconstructor));

            var changes = new TTarget()
            {
                LogicalName = target.LogicalName,
                Id = target.Id
            };

            foreach (var (key, sValue) in deconstructor.GetAttributeValues(source))
            {
                bool keyExists = target.Attributes.TryGetValue(key, out object tValue);
                bool valuesAreNotEqual = ShouldTrackChange(sValue, tValue);

                if (keyExists && valuesAreNotEqual)
                {
                    changes[key] = sValue;
                }
            }

            return changes;
        }

        // TODO: time to update to support OptionSetValueCollection attributes?
        // TODO: tests
        internal static bool ShouldTrackChange(object sValue, object tValue)
        {
            if (tValue == null && sValue == null)
                return false;

            var obj = sValue ?? tValue;

            switch (obj)
            {
                // Guid (not EntityReference) attributes are used for primary keys or different internal staff
                // such as address1(2,3)_addressid, entityimageid, businessprocessflowinstanceid, processid, stageid,
                // azureactivedirectoryobjectid, conversationtrackingid, privilegeusergroupid, etc
                // You got it. Most likely, this attributes will be ignored during Update, otherwise they should be
                // used with care and be set manually
                case Guid _:
                    return false;

                // We don't want to break concurrency behavior with RowVersion attribute, or EntityImage_Timestamp or
                // other internal stuff. BigInt (long) attributes are internal only according to documentation
                case long _:
                    return false;

                // EntityImage attributes are byte arrays (byte[]) while annotation bodies are strings
                // technically we should use EntityImage_Timestamp attribute to check if there are changes
                // but it was ignored in previous step
                // Note that v9.1+ file & image attributes can't be compared this way
                case byte[] _:
                    return !ByteArraysAreEqual((byte[])tValue, (byte[])sValue);

                case EntityCollection _:
                    return !CollectionsAreEqual((EntityCollection)tValue, (EntityCollection)sValue);

                default:
                    return !(tValue?.Equals(sValue) == true);
            }
        }

        // EntityCollection attributes are almost always PartyList's (with rare exceptions such as CalendarRules)
        // ActivityParty entities are mostly immutable, its attributes can't be changed with UI, the only thing
        // that can be changed is a list of parties itself
        // TODO: empty collection instead of null
        private static bool CollectionsAreEqual(EntityCollection tValue, EntityCollection sValue)
        {
            if (tValue == null && sValue == null)
                return true;

            //TODO: should empty collections be eq to null?
            if (tValue == null ^ sValue == null)
                return false;

            if (tValue.Entities.Count != sValue.Entities.Count)
                return false;

            // we expect short lists, so sorting shouldn't be too expensive
            var tIds = tValue.Entities.Select(e => e.Id).OrderBy(id => id);
            var sIds = tValue.Entities.Select(e => e.Id).OrderBy(id => id);

            return tIds.SequenceEqual(sIds);
        }

        private static bool ByteArraysAreEqual(byte[] b1, byte[] b2)
        {
            if (b1 == null && b2 == null)
                return true;

            if (b1 == null ^ b2 == null)
                return false;

            if (b1.Length != b2.Length)
                return false;

            return b1.SequenceEqual(b2);
        }
    }
}
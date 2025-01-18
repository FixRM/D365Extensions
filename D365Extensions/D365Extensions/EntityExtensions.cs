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
        /// Simplifies getting values from Entity.FormattedValues collection
        /// <param name="attributeLogicalName">Attribute name</param>
        /// <param name="attributeLogicalName">Entity alias</param>
        /// <returns>Attribute formated value</returns>
        public static string GetFormatedValue(this Entity entity, string attributeLogicalName, string alias)
        {
            CheckParam.CheckForNull(attributeLogicalName, nameof(attributeLogicalName));
            CheckParam.CheckForNull(attributeLogicalName, nameof(alias));

            entity.FormattedValues.TryGetValue($"{alias}.{attributeLogicalName}", out string outValue);
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
        public static Entity GetAliasedEntity(this Entity entity, string entityLogicalName, string alias = null)
        {
            CheckParam.CheckForNull(entityLogicalName, nameof(entityLogicalName));

            /// Use LogicalName as alias if it is not specified
            string aliasPrefix = (alias ?? entityLogicalName) + ".";

            var aliasedAttributes = entity.Attributes.Where(a => a.Key.StartsWith(aliasPrefix))
                .Select(a => a.Value as AliasedValue)
                .Where(a => a != null)
                .Select(a => new KeyValuePair<string, object>(a.AttributeLogicalName, a.Value));

            var aliasedFormatedValues = entity.FormattedValues.Where(a => a.Key.StartsWith(aliasPrefix))
                .Select(a => new KeyValuePair<string, string>(GetAttributePart(a.Key), a.Value));

            Entity aliasedEntity = new Entity(entityLogicalName);
            aliasedEntity.Attributes.AddRange(aliasedAttributes);
            aliasedEntity.FormattedValues.AddRange(aliasedFormatedValues);

            return aliasedEntity;
        }

        private static string GetAttributePart(string name)
        {
            return name.Substring(name.IndexOf(".") + 1);
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
        /// <param name="newRefs">Create new EntityReference values instead of taking from source entity. In rare cases, messages like MergeRequest can fail if entityReference.Name is specified</param>
        public static void MergeAttributes(this Entity target, Entity source, bool newRefs = false)
        {
            if (source != null)
            {
                foreach (var attribute in source.Attributes)
                {
                    if (target.Attributes.ContainsKey(attribute.Key)) continue;

                    if (newRefs && attribute.Value is EntityReference reference)
                    {
                        var newReference = new EntityReference(reference.LogicalName, reference.Id);

                        target.Attributes.Add(attribute.Key, newReference);
                    }
                    else
                    {
                        target.Attributes.Add(attribute);
                    }
                }

                foreach (var formattedValue in source.FormattedValues)
                {
                    if (target.FormattedValues.ContainsKey(formattedValue.Key)) continue;

                    target.FormattedValues.Add(formattedValue);
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

        /// <summary>
        /// Remove attributes form target Entity if they have the same value in source Entity
        /// 
        /// Note that EntityCollection attributes are compared only by Entity Id's
        /// </summary>
        /// <param name="source">Entity to compare attributes with</param>
        public static void RemoveUnchanged(this Entity target, Entity source)
        {
            CheckParam.CheckForNull(source, nameof(source));

            foreach (var key in target.Attributes.Keys.ToArray())
            {
                if (AreEqual(target[key], source.GetAttributeValue<object>(key)))
                {
                    target.Attributes.Remove(key);
                }
            }
        }

        internal static bool AreEqual(object sValue, object tValue)
        {
            if (tValue == null && sValue == null)
                return true;

            var obj = sValue ?? tValue;

            switch (obj)
            {
                // Guid (not EntityReference) attributes are used for primary keys or different internal staff
                // such as address1(2,3)_addressid, entityimageid, businessprocessflowinstanceid, processid, stageid,
                // azureactivedirectoryobjectid, conversationtrackingid, privilegeusergroupid, etc
                // You got it. Most likely, this attributes will be ignored during Update, otherwise they should be
                // used with care and be set manually
                //case Guid _:
                //    return true;

                // BigInt (long) attributes are internal only according to documentation
                // We don't want to break concurrency behavior with RowVersion attribute, or EntityImage_Timestamp or
                // other internal stuff. 
                //case long _:
                //    return true;

                // EntityImage attributes are byte arrays (byte[]) while Annotation bodies are strings
                // Technically we should use EntityImage_Timestamp (BigInt/long) attribute to check if there are changes
                // Note that v9.1+ file & image attributes can't be compared this way
                case byte[] _:
                    return ByteArraysAreEqual((byte[])tValue, (byte[])sValue);

                // EntityCollection attributes are almost always PartyList's (with rare exceptions such as CalendarRules)
                // ActivityParty entities are mostly immutable, its attributes can't be changed with UI, the only thing
                // that can be changed is a list of parties itself
                case EntityCollection _:
                    return CollectionsAreEqual((EntityCollection)tValue, (EntityCollection)sValue);

                default:
                    //return tValue?.Equals(sValue) == true;
                    return Equals(tValue, sValue);
            }
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
            var sIds = sValue.Entities.Select(e => e.Id).OrderBy(id => id);

            return tIds.SequenceEqual(sIds);
        }
    }
}
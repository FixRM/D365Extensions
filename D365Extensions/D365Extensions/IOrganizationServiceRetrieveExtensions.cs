﻿using D365Extensions;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq.Expressions;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Extensions for IOrganizationService.Retrieve
    /// </summary>
    public static partial class IOrganizationServiceExtensions
    {
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
        /// Retrieve method override. Returns strongly typed entity object
        /// </summary>
        /// <param name="id">The Id of record that you want to retrieve</param>
        public static T Retrieve<T>(this IOrganizationService service, Guid id, ColumnSet columnSet) where T : Entity
        {
            Entity entity = service.Retrieve(LogicalName.GetName<T>(), id, columnSet);

            return entity.ToEntity<T>();
        }

        /// <summary>
        /// Retrieve method override. Returns strongly typed entity object
        /// </summary>
        /// <param name="id">The Id of record that you want to retrieve</param>
        public static T Retrieve<T>(this IOrganizationService service, Guid id, params string[] columns) where T : Entity
        {
            return service.Retrieve<T>(id, new ColumnSet(columns));
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

            KeyAttributeCollection keys = new KeyAttributeCollection
            {
                { keyName, keyValue }
            };

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
        [Obsolete("Use Retrieve<T>(this IOrganizationService service, string keyName, object keyValue, ColumnSet columnSet)")]
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
        public static T Retrieve<T>(this IOrganizationService service, string keyName, object keyValue, ColumnSet columnSet) where T : Entity
        {
            Entity entity = service.Retrieve(LogicalName.GetName<T>(), keyName, keyValue, columnSet);

            return entity.ToEntity<T>();
        }

        /// <summary>
        /// Retrieve method override. Retrieves by Alternative key and returns strongly typed entity object
        /// </summary>
        /// <param name="keyName">Name of alternative key</param>
        /// <param name="keyValue">Key value</param>
        [Obsolete("Use Retrieve<T>(this IOrganizationService service, string keyName, object keyValue, params string[] columns)")]
        public static T Retrieve<T>(this IOrganizationService service, string logicalName, string keyName, string keyValue, params String[] columns) where T : Entity
        {
            return service.Retrieve<T>(logicalName, keyName, keyValue, new ColumnSet(columns));
        }

        /// <summary>
        /// Retrieve method override. Retrieves by Alternative key and returns strongly typed entity object
        /// </summary>
        /// <param name="keyName">Name of alternative key</param>
        /// <param name="keyValue">Key value</param>
        public static T Retrieve<T>(this IOrganizationService service, string keyName, object keyValue, params string[] columns) where T : Entity
        {
            return service.Retrieve<T>(keyName, keyValue, new ColumnSet(columns));
        }

        /// <summary>
        /// Retrieve method override. Retrieves by Alternative key and returns strongly typed entity object
        /// </summary>
        /// <param name="keyName">Name of alternative key</param>
        /// <param name="keyValue">Key value</param>
        /// <param name="columns">Columns to retrieve</param>
        public static T Retrieve<T>(this IOrganizationService service, Expression<Func<T, object>> keyName, object keyValue, params Expression<Func<T, object>>[] columns) where T : Entity
        {
            var columnSet = new ColumnSet();
            columnSet.Columns.AddRange(LogicalName.GetNames(columns));

            return service.Retrieve<T>(LogicalName.GetName(keyName), keyValue, columnSet);
        }

        /// <summary>
        /// Retrieve method override. Returns strongly typed entity object
        /// </summary>
        /// <param name="id">The Id of record that you want to retrieve</param>
        public static T Retrieve<T>(this IOrganizationService service, Guid id, params Expression<Func<T, object>>[] columns) where T : Entity
        {
            var columnSet = new ColumnSet();
            columnSet.Columns.AddRange(LogicalName.GetNames(columns));

            return service.Retrieve<T>(id, columnSet);
        }

        /// <summary>
        /// Retrieve method override. Returns strongly typed entity object
        /// </summary>
        /// <param name="reference">Entity to retrieve</param>
        public static T Retrieve<T>(this IOrganizationService service, EntityReference reference, params Expression<Func<T, object>>[] columns) where T : Entity
        {
            var columnSet = new ColumnSet();
            columnSet.Columns.AddRange(LogicalName.GetNames(columns));

            return service.Retrieve<T>(reference, columnSet);
        }
    }
}
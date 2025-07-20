using D365Extensions;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Extensions for IOrganizationService.RetrieveMultiple
    /// </summary>
    public static partial class IOrganizationServiceExtensions
    {
        /// <summary>
        /// Universal RetrieveMultiple method override. Returns all pages using callback or 'yield' iterator
        /// </summary>
        /// <param name="query">A query that determines the set of record</param>
        /// <param name="callback">Optional function to be called for each record page</param>
        /// <returns>Entity set as 'yield' iterator</returns>
        public static IEnumerable<Entity> RetrieveMultiple(this IOrganizationService service, QueryBase query, Action<EntityCollection> callback = null)
        {
            CheckParam.CheckForNull(query, nameof(query));
            CheckParam.CheckPageNumber(query);

            EntityCollection collection = new EntityCollection
            {
                MoreRecords = true
            };

            while (collection.MoreRecords)
            {
                /// Paging start working if Page > 0
                query.NextPage(collection.PagingCookie);

                collection = service.RetrieveMultiple(query);
                callback?.Invoke(collection);

                foreach (Entity entity in collection.Entities)
                {
                    yield return entity;
                }
            }
        }

        /// <summary>
        /// RetrieveMultiple method override optimized for FetchExpression. Returns all pages using callback or 'yield' iterator
        /// </summary>
        /// <param name="query">A query that determines the set of record</param>
        /// <param name="callback">Optional function to be called for each record page</param>
        /// <returns>Entity set as 'yield' iterator</returns>
        public static IEnumerable<Entity> RetrieveMultiple(this IOrganizationService service, FetchExpression query, Action<EntityCollection> callback = null)
        {
            CheckParam.CheckForNull(query, nameof(query));

            /// For performance reasons it's better to load XML once
            XDocument document = XDocument.Parse(query.Query);

            CheckParam.CheckPageNumber(query, document);

            EntityCollection collection = new EntityCollection
            {
                MoreRecords = true
            };

            while (collection.MoreRecords)
            {
                /// Paging start working if Page > 0
                query.NextPage(document, collection.PagingCookie);

                collection = service.RetrieveMultiple(query);
                callback?.Invoke(collection);

                foreach (Entity entity in collection.Entities)
                {
                    yield return entity;
                }
            }
        }

        /// <summary>
        /// RetrieveMultiple method override optimized for FetchExpression. Returns all pages using callback or 'yield' iterator
        /// </summary>
        /// <param name="query">A query in fetch XML</param>
        /// <param name="callback">Optional function to be called for each record page</param>
        /// <returns>Entity set as 'yield' iterator</returns>
        public static IEnumerable<Entity> RetrieveMultiple(this IOrganizationService service, string fetchXml, Action<EntityCollection> callback = null)
        {
            CheckParam.CheckForNull(fetchXml, nameof(fetchXml));

            return service.RetrieveMultiple(new FetchExpression(fetchXml), callback);
        }

        /// <summary>
        /// Retrieves a collection of records
        /// </summary>
        /// <param name="fetchXml">A query in fetch XML</param>
        /// <returns>EntityCollection</returns>
        public static EntityCollection RetrieveMultiple(this IOrganizationService service, string fetchXml)
        {
            CheckParam.CheckForNull(fetchXml, nameof(fetchXml));

            return service.RetrieveMultiple(new FetchExpression(fetchXml));
        }

        /// <summary>
        /// Universal RetrieveMultiple method override. Returns all pages using callback or 'yield' iterator
        /// 
        /// WARNING: for performance reasons you should use this override only if proxy types are enabled for your
        /// plugin assembly (via ProxyTypesAssembly attribute) or application (using EnableProxyTypes() method)
        /// </summary>
        /// <param name="query">A query that determines the set of record</param>
        /// <param name="callback">Optional function to be called for each record page</param>
        /// <returns>Entity set as 'yield' iterator</returns>
        public static IEnumerable<T> RetrieveMultiple<T>(this IOrganizationService service, QueryBase query, Action<EntityCollection> callback = null) where T : Entity
        {
            return service.RetrieveMultiple(query, callback).Select(e => e.ToEntity<T>());
        }

        /// <summary>
        /// RetrieveMultiple method override optimized for FetchExpression. Returns all pages using callback or 'yield' iterator
        /// 
        /// WARNING: for performance reasons you should use this override only if proxy types are enabled for your
        /// plugin assembly (via ProxyTypesAssembly attribute) or application (using EnableProxyTypes() method)
        /// </summary>
        /// <param name="query">A query that determines the set of record</param>
        /// <param name="callback">Optional function to be called for each record page</param>
        /// <returns>Entity set as 'yield' iterator</returns>
        public static IEnumerable<T> RetrieveMultiple<T>(this IOrganizationService service, FetchExpression query, Action<EntityCollection> callback = null) where T: Entity
        {
            return service.RetrieveMultiple(query, callback).Select(e => e.ToEntity<T>());
        }

        /// <summary>
        /// Retrieves single query result as Entity or throws InvalidOperationException 
        /// if query returns nothing or more than one record
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Entity</returns>
        /// <exception cref="InvalidOperationException"/>
        public static Entity RetrieveSingle(this IOrganizationService service, QueryBase query)
        {
            query.SetTopCount(2);

            var results = service.RetrieveMultiple(query);

            return results.Entities.Single();
        }

        /// <summary>
        /// Retrieves single query result as as strongly typed entity object or throws
        /// InvalidOperationException if query returns nothing or more than one record
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Strongly typed entity object</returns>
        /// <exception cref="InvalidOperationException"/>

        public static T RetrieveSingle<T>(this IOrganizationService service, QueryBase query) where T : Entity
        {
            return service.RetrieveSingle(query).ToEntity<T>();
        }

        /// <summary>
        /// Retrieves single query result as Entity or null if query did not return results
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Entity</returns>
        public static Entity RetrieveSingleOrDefault(this IOrganizationService service, QueryBase query)
        {
            query.SetTopCount(1);

            var results = service.RetrieveMultiple(query);

            return results.Entities.SingleOrDefault();
        }

        /// <summary>
        /// Retrieves single query result as strongly typed entity object 
        /// or null if query did not return results
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Strongly typed entity object</returns>

        public static T RetrieveSingleOrDefault<T>(this IOrganizationService service, QueryBase query) where T : Entity
        {
            return service.RetrieveSingleOrDefault(query)?.ToEntity<T>();
        }
    }
}
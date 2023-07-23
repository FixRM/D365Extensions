using D365Extensions;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
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

            return RetrieveMultiple(service, new FetchExpression(fetchXml), callback);
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
    }
}

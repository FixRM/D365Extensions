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

            EntityCollection collection = new EntityCollection
            {
                MoreRecords = true
            };

            while (collection.MoreRecords)
            {
                /// Paging start working if Page > 1
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

            EntityCollection collection = new EntityCollection
            {
                MoreRecords = true
            };

            /// For performance reasons it's better to load XML once
            XDocument document = XDocument.Parse(query.Query);

            while (collection.MoreRecords)
            {
                /// Paging start working if Page > 1
                query.NextPage(document, collection.PagingCookie);

                collection = service.RetrieveMultiple(query);
                callback?.Invoke(collection);

                foreach (Entity entity in collection.Entities)
                {
                    yield return entity;
                }
            }
        }
    }
}

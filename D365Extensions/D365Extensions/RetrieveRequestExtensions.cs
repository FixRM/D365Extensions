using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk.Messages
{
    /// <summary>
    /// Shortcuts for RelationshipQueryCollectionExtension
    /// </summary>
    public static class RetrieveRequestExtensions
    {
        /// <summary>
        /// Adds relationship-query pair.
        /// </summary>
        /// <param name="relationshipName">Relationship schema name</param>
        /// <param name="query">Query to retrieve related entities</param>
        public static void AddRelated(this RetrieveRequest request, string relationshipName, QueryBase query)
        {
            request.RelatedEntitiesQuery.Add(relationshipName, query);
        }

        /// <summary>
        /// Adds simple query without conditions
        /// </summary>
        /// <param name="relationshipName">Relationship schema name</param>
        /// <param name="entityLogicalName">Logical name of the entity for the query</param>
        /// <param name="columnSet">Columns to include</param>
        public static void Add(this RetrieveRequest request, string relationshipName, string entityLogicalName, ColumnSet columnSet)
        {
            request.RelatedEntitiesQuery.Add(relationshipName, entityLogicalName, columnSet);
        }

        /// <summary>
        /// Adds relationship-query pair.
        /// </summary>
        /// <param name="relationshipName">Relationship schema name expression</param>
        /// <param name="query">Query to retrieve related entities</param>
        public static void AddRelated<T>(this RetrieveRequest request, Expression<Func<T, object>> relationshipName, QueryBase query) where T : Entity
        {
            request.RelatedEntitiesQuery.Add<T>(relationshipName, query);
        }

        /// <summary>
        /// Adds simple query without conditions
        /// </summary>
        /// <param name="relationshipName">Relationship schema name expression</param>
        /// <param name="columnSet">Columns to include</param>
        public static void AddRelated<T>(this RetrieveRequest request, Expression<Func<T, object>> relationshipName, ColumnSet columnSet) where T : Entity
        {
            request.RelatedEntitiesQuery.Add<T>(relationshipName, columnSet);
        }
    }
}

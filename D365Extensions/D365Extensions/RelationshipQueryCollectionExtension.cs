using D365Extensions;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq.Expressions;

namespace Microsoft.Xrm.Sdk;

public static class RelationshipQueryCollectionExtension
{
    /// <summary>
    /// Adds relationship-query pair.
    /// </summary>
    /// <param name="relationshipName">Relationship schema name</param>
    /// <param name="query">Query to retrieve related entities</param>
    public static void Add(this RelationshipQueryCollection collection, string relationshipName, QueryBase query)
    {
        var relationship = new Relationship(relationshipName);

        collection.Add(relationship, query);
    }

    /// <summary>
    /// Adds simple query without conditions
    /// </summary>
    /// <param name="relationshipName">Relationship schema name</param>
    /// <param name="entityLogicalName">Logical name of the entity for the query</param>
    /// <param name="columnSet">Columns to include</param>
    public static void Add(this RelationshipQueryCollection collection, string relationshipName, string entityLogicalName, ColumnSet columnSet)
    {
        var relationship = new Relationship(relationshipName);

        QueryExpression query = new()
        {
            EntityName = entityLogicalName,
            ColumnSet = columnSet
        };

        collection.Add(relationship, query);
    }

    /// <summary>
    /// Adds relationship-query pair.
    /// </summary>
    /// <param name="relationshipName">Relationship schema name expression</param>
    /// <param name="query">Query to retrieve related entities</param>
    public static void Add<T>(this RelationshipQueryCollection collection, Expression<Func<T, object>> relationshipName, QueryBase query) where T : Entity
    {
        var info = LogicalName.GetRelationshipInfo(relationshipName);

        var relationship = new Relationship()
        {
            SchemaName = info.SchemaName,
            PrimaryEntityRole = info.PrimaryEntityRole,
        };

        collection.Add(relationship, query);
    }

    /// <summary>
    /// Adds simple query without conditions
    /// </summary>
    /// <param name="relationshipName">Relationship schema name expression</param>
    /// <param name="columnSet">Columns to include</param>
    public static void Add<T>(this RelationshipQueryCollection collection, Expression<Func<T, object>> relationshipName, ColumnSet columnSet) where T : Entity
    {
        var info = LogicalName.GetRelationshipInfo(relationshipName);

        var relationship = new Relationship()
        {
            SchemaName = info.SchemaName,
            PrimaryEntityRole = info.PrimaryEntityRole,
        };

        QueryExpression query = new()
        {
            EntityName = info.RelatedEntityName,
            ColumnSet = columnSet
        };

        collection.Add(relationship, query);
    }
}


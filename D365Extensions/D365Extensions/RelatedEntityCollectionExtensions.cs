using D365Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk;

public static class RelatedEntityCollectionExtensions
{
    /// <summary>
    /// Get collection of related entities. Don`t use it with EB classes, use code generated relationship properties insted.
    /// </summary>
    /// <param name="relationshipSchemaName">Relationship name</param>
    /// <param name="primaryEntityRole">Must be specified for self-referenced relationships. 'Referencing' for N:1 
    /// and 'Referenced' for 1:N relationships</param>
    /// <returns>null if collection is not found</returns>
    public static EntityCollection Get(this RelatedEntityCollection relatedEntities,
        string relationshipSchemaName,
        EntityRole? primaryEntityRole = null)
    {
        CheckParam.CheckForNull(relationshipSchemaName, nameof(relationshipSchemaName));

        Relationship relationship = new Relationship()
        {
            SchemaName = relationshipSchemaName,
            PrimaryEntityRole = primaryEntityRole
        };

        relatedEntities.TryGetValue(relationship, out var entityCollection);

        return entityCollection;
    }

    /// <summary>
    /// Add entity to related entity collection. If collection is not found, it will be created. Don`t use it with EB classes, use code generated relationship properties insted.
    /// </summary>
    /// <param name="relationship">Schema name of relationship with current entity</param>
    /// <param name="entity">Entity to add.</param>
    public static void AddTo(this RelatedEntityCollection relatedEntities, string relationship, Entity entity)
    {
        CheckParam.CheckForNull(relationship, nameof(relationship));

        relatedEntities.AddTo(new Relationship(relationship), entity);
    }

    /// <summary>
    /// Add entity to related entity collection. If collection is not found, it will be created. Don`t use it with EB classes, use code generated relationship properties insted.
    /// </summary>
    /// <param name="relationship">Relationship with current entity</param>
    /// <param name="entity">Entity to add.</param>
    public static void AddTo(this RelatedEntityCollection relatedEntities, Relationship relationship, Entity entity)
    {
        CheckParam.CheckForNull(relationship, nameof(relationship));
        CheckParam.CheckForNull(entity, nameof(entity));

        EntityCollection entityCollection = relatedEntities.GetOrAdd(relationship);

        entityCollection.Entities.Add(entity);
    }

    /// <summary>
    /// Add entities to related entity collection. If collection is not found, it will be created. Don`t use it with EB classes, use code generated relationship properties insted.
    /// </summary>
    /// <param name="relationship">Schema name of relationship with current entity</param>
    /// <param name="entity">Entities to add.</param>
    public static void AddTo(this RelatedEntityCollection relatedEntities, string relationship, IEnumerable<Entity> entities)
    {
        CheckParam.CheckForNull(relationship, nameof(relationship));

        relatedEntities.AddTo(new Relationship(relationship), entities);
    }

    /// <summary>
    /// Add entities to related entity collection. If collection is not found, it will be created. Don`t use it with EB classes, use code generated relationship properties insted.
    /// </summary>
    /// <param name="relationship">Relationship with current entity</param>
    /// <param name="entity">Entities to add.</param>
    public static void AddTo(this RelatedEntityCollection relatedEntities, Relationship relationship, IEnumerable<Entity> entities)
    {
        CheckParam.CheckForNull(relationship, nameof(relationship));
        CheckParam.CheckForNull(entities, nameof(entities));

        EntityCollection entityCollection = relatedEntities.GetOrAdd(relationship);

        entityCollection.Entities.AddRange(entities);
    }

    /// <summary>
    /// Helper method to initialize related entity collection before adding members if needed.
    /// </summary>
    internal static EntityCollection GetOrAdd(this RelatedEntityCollection relatedEntities, Relationship relationship)
    {
        CheckParam.CheckForNull(relationship, nameof(relationship));

        if (!relatedEntities.TryGetValue(relationship, out var entityCollection))
        {
            entityCollection = new EntityCollection();
            relatedEntities.Add(relationship, entityCollection);
        }

        return entityCollection;
    }
}
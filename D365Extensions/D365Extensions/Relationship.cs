using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk;


/// <summary>
/// Strongly typed version of the Relationship class.
/// </summary>
public sealed class Relationship<T> where T : Entity
{
    /// <summary>
    /// Relationship has relatevly complex overrides of Equals and GetHashCode
    /// So unlike other strongly typed versions in this library, it's easier to have this one to be
    /// a wrapper of OOB type instead of independent implementation
    /// </summary>
    private readonly Relationship relationship;

    /// <summary>
    ///  Gets or sets the name of the relationship.
    /// </summary>
    public string SchemaName
    {
        get
        {
            return relationship.SchemaName;
        }
        set
        {
            this.relationship.SchemaName = value;
        }
    }

    public string RelatedEntityName { get; internal set; }

    /// <summary>
    /// Gets or sets the entity role: referenced or referencing.
    /// </summary>
    public EntityRole? PrimaryEntityRole
    {
        get
        {
            return relationship.PrimaryEntityRole;
        }
        set
        {
            this.relationship.PrimaryEntityRole = value;
        }
    }

    public Relationship()
    {
        this.relationship = new Relationship();
    }

    public Relationship(Expression<Func<T, object>> schemaName)
    {
        var relationshipInfo = LogicalName.GetRelationshipInfo(schemaName);

        this.relationship = new Relationship()
        {
            PrimaryEntityRole = relationshipInfo.PrimaryEntityRole,
            SchemaName = relationshipInfo.SchemaName,
        };

        this.RelatedEntityName = relationshipInfo.RelatedEntityName;
    }

    public static implicit operator Relationship(Relationship<T> t)
    {
        if (t is null) return null;        

        return t.relationship;
    }

    /// <summary>
    /// WARNING: Relationship and Relationship<T> are different types! You can't compare them directly!
    /// You should always explicitly cast Relationship<T> to Relationship before comparison:
    /// <code>
    /// relationship.Equals((Relationship) relationshipT)
    /// </code>
    /// </summary>
    public override bool Equals(object obj)
    {
        if (obj is not Relationship<T> relationship)
        {
            return false;
        }

        if (RelatedEntityName != relationship.RelatedEntityName) 
            return false;

        return this.relationship.Equals(relationship.relationship);
    }

    public override int GetHashCode()
    {
        var code = (this.RelatedEntityName ?? String.Empty).GetHashCode();
        code ^= this.relationship.GetHashCode();

        return code;
    }

    public override string ToString()
    {
        return this.relationship.ToString();
    }
}

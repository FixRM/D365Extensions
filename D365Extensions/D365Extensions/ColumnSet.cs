using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using D365Extensions;

namespace Microsoft.Xrm.Sdk.Query
{
    /// <summary>
    /// Strongly typed version of the ColumnSet class
    /// </summary>
    public sealed class ColumnSet<T> where T : Entity
    {
        /// <summary>
        /// Gets the collection of lambdas containing the names of the attributes to be retrieved
        /// from a query.
        /// </summary>
        public List<Expression<Func<T, object>>> Columns { get; } = new List<Expression<Func<T, object>>>();

        /// <summary>
        /// Initializes a new instance of the ColumnSet<T> class.
        /// </summary>
        public ColumnSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ColumnSet<T> class setting the Columns property.
        /// </summary>
        /// <param name="columns">Specifies an array of property expressions containing the names of the attributes.
        /// </param>
        public ColumnSet(params Expression<Func<T, object>>[] columns)
        {
            Columns.AddRange(columns);
        }

        /// <summary>
        /// Adds the specified attribute to the column set
        /// </summary>
        /// <param name="column">Specifies a property expressions containing the name of the attribute.</param>
        public void AddColumn(Expression<Func<T, object>> column)
        {
            Columns.Add(column);
        }

        /// <summary>
        /// Adds the specified attributes to the column set.
        /// </summary>
        /// <param name="columns">Specifies an array of property expressions containing the names of the attributes.</param>
        public void AddColumns(params Expression<Func<T, object>>[] columns)
        {
            Columns.AddRange(columns);
        }

        /// <summary>
        /// Converts ColumnSet<T> to ColumnSet
        /// </summary>
        public static implicit operator ColumnSet(ColumnSet<T> t)
        {
            if (t == null) return null;

            return new ColumnSet(LogicalName.GetNames(t.Columns.ToArray()));
        }
    }
}
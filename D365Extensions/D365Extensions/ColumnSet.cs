using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using D365Extensions;

namespace Microsoft.Xrm.Sdk.Query
{
    /// <summary>
    /// Strongly typed version of the ColumnSet class
    /// </summary>
    public class ColumnSet<T> where T : Entity
    {
        /// <summary>
        /// Gets the collection of Strings containing the names of the attributes to be retrieved
        /// from a query.
        /// </summary>
        public List<string> Columns { get; } = new List<string>();

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
            Columns.AddRange(LogicalName.GetNames(columns));
        }

        /// <summary>
        /// Adds the specified attribute to the column set
        /// </summary>
        /// <param name="column">Specifies a property expressions containing the name of the attribute.</param>
        public void AddColumn(Expression<Func<T, object>> column)
        {
            Columns.Add(LogicalName.GetName(column));
        }

        /// <summary>
        /// Adds the specified attributes to the column set.
        /// </summary>
        /// <param name="columns">Specifies an array of property expressions containing the names of the attributes.</param>
        public void AddColumns(params Expression<Func<T, object>>[] columns)
        {
            Columns.AddRange(LogicalName.GetNames(columns));
        }

        /// <summary>
        /// Converts ColumnSet<T> to ColumnSet
        /// </summary>
        public static implicit operator ColumnSet(ColumnSet<T> t)
        {
            return new ColumnSet(t.Columns.ToArray());
        }
    }
}

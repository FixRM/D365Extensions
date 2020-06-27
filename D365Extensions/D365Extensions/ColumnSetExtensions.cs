using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
        /// <param name="expressions">Specifies an array of property expressions containing the names of the attributes.
        /// </param>
        public ColumnSet(params Expression<Func<T, object>>[] expressions)
        {
            Columns = ProperyExpression.GetNames(expressions);
        }

        /// <summary>
        /// Adds the specified attribute to the column set
        /// </summary>
        /// <param name="expression">Specifies a property expressions containing the name of the attribute.</param>
        public void AddColumn(Expression<Func<T, object>> expression)
        {
            Columns.Add(ProperyExpression.GetName(expression));
        }

        /// <summary>
        /// Adds the specified attributes to the column set.
        /// </summary>
        /// <param name="expressions">Specifies an array of property expressions containing the names of the attributes.</param>
        public void AddColumns(params Expression<Func<T, object>>[] expressions)
        {
            Columns.AddRange(ProperyExpression.GetNames(expressions));
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

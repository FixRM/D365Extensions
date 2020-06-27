using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using D365Extensions;

namespace Microsoft.Xrm.Sdk.Query
{
    /// <summary>
    /// Strongly typed version of ColumnSet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ColumnSet<T> where T : Entity
    {
        public List<string> Columns { get; } = new List<string>();

        public ColumnSet()
        {
        }

        public ColumnSet(params Expression<Func<T, object>>[] expressions)
        {
            Columns = ProperyExpression.GetNames(expressions);
        }

        public void AddColumn(Expression<Func<T, object>> expression)
        {
            Columns.Add(ProperyExpression.GetName(expression));
        }

        public void AddColumns(params Expression<Func<T, object>>[] expressions)
        {
            Columns.AddRange(ProperyExpression.GetNames(expressions));
        }

        public static implicit operator ColumnSet(ColumnSet<T> t)
        {
            return new ColumnSet(t.Columns.ToArray());
        }
    }
}

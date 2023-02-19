using D365Extensions;
using System;
using System.Linq.Expressions;

namespace Microsoft.Xrm.Sdk.Query
{
    public static class ColumnSetExtensions
    {
        /// <summary>
        /// Adds the specified attribute to the column set.
        /// </summary>
        /// <typeparam name="T">Type of the entity to add column from</typeparam>
        /// <param name="column">The property expression containing the name of the attribute to add</param>
        public static void AddColumn<T>(this ColumnSet columnSet, Expression<Func<T, object>> column) where T: Entity
        {
            columnSet.AddColumn(LogicalName.GetName(column));
        }

        /// <summary>
        /// Adds the specified attributes to the column set.
        /// </summary>
        /// <typeparam name="T">Type of the entity to add column from</typeparam>
        /// <param name="column">The property expressions containing the name of the attribute to add</param>
        public static void AddColumns<T>(this ColumnSet columnSet, params Expression<Func<T, object>>[] columns) where T : Entity
        {
            columnSet.AddColumns(LogicalName.GetNames(columns));
        }
    }
}

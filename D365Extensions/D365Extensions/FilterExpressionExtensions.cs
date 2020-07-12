using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using D365Extensions;

namespace Microsoft.Xrm.Sdk.Query
{
    /// <summary>
    /// Extensions for FilterExpression
    /// </summary>
    public static class FilterExpressionExtensions
    {
        /// <summary>
        /// Adds a condition to the filter expression setting the attribute name, condition operator, and value array.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="attributeName">Property expressions containing the name of the attribute.</param>
        /// <param name="conditionOperator">Condition operator.</param>
        /// <param name="values">The array of values to add.</param>
        public static void AddCondition<T>(this FilterExpression filterExpression, Expression<Func<T, object>> attributeName,  ConditionOperator conditionOperator, params object[] values) where T : Entity
        {
            filterExpression.AddCondition(ProperyExpression.GetName(attributeName), conditionOperator, values);
        }

        /// <summary>
        /// Adds a condition to the filter expression setting the entity name, attribute name, condition operator, and value array.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="attributeName">Property expressions containing the name of the attribute.</param>
        /// <param name="conditionOperator">Condition operator.</param>
        /// <param name="values">The array of values to add.</param>
        public static void AddCondition<T>(this FilterExpression filterExpression, string entityName, Expression<Func<T, object>> attributeName,  ConditionOperator conditionOperator,  params object[] values) where T : Entity
        {
            filterExpression.AddCondition(entityName, ProperyExpression.GetName(attributeName), conditionOperator, values);
        }
    }
}

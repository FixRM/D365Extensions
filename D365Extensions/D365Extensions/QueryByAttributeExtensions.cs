using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk.Query
{
    /// <summary>
    /// Extensions for QueryByAttribute
    /// </summary>
    public static class QueryByAttributeExtensions
    {
        /// <summary>
        /// Adds an attribute value to the attributes collection.
        /// </summary>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <param name="attributeName">The property expressions containing the name of the attribute</param>
        /// <param name="value">The attribute value.</param>
        public static void AddAttribute<T>(this QueryByAttribute query, Expression<Func<T, object>> attributeName, object value) where T : Entity
        {
            query.AddAttributeValue(LogicalName.GetName(attributeName), value);
        }

        /// <summary>
        /// Adds an order to the orders collection.
        /// </summary>
        /// <typeparam name="T">Type of the entity</typeparam>
        /// <param name="attributeName">The property expressions containing the name of the attribute</param>
        /// <param name="orderType">The order for that attribute.</param>
        public static void AddOrder<T>(this QueryByAttribute query, Expression<Func<T, object>> attributeName, OrderType orderType) where T : Entity
        {
            query.AddOrder(LogicalName.GetName(attributeName), orderType);
        }
    }
}

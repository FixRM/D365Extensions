using D365Extensions;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk.Query
{
    /// <summary>
    /// Strongly typed version of the OrderExpression class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrderExpression<T> where T: Entity
    {
        /// <summary>
        /// Initializes a new instance of the OrderExpression<T> class.
        /// </summary>
        public OrderExpression()
        {
        }

        /// <summary>
        /// Initializes a new instance of the OrderExpression<T> class 
        /// setting the attribute name and the order type.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <param name="orderType">The order, ascending or descending.</param>
        public OrderExpression(Expression<Func<T, object>> attributeName, OrderType orderType)
        {
            AttributeName = attributeName;
            OrderType = OrderType;
        }

        /// <summary>
        /// Gets or sets the name of the attribute in the order expression.
        /// </summary>
        public Expression<Func<T, object>> AttributeName { get; set; }

        /// <summary>
        /// Gets or sets the order, ascending or descending.
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// Converts OrderExpression<T> to OrderExpression
        /// </summary>
        public static implicit operator OrderExpression(OrderExpression<T> t)
        {
            return new OrderExpression(ProperyExpression.GetName(t.AttributeName), t.OrderType);
        }
    }
}

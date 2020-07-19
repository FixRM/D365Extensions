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
    /// Extensions for QueryExpression
    /// </summary>
    public static class QueryExpressionExtensions
    {
        /// <summary>
        /// Adds the specified order expression to the query expression.
        /// </summary>
        /// <param name="attributeName">The property expressions containing the name of the attribute</param>
        /// <param name="orderType">The order for that attribute.</param>
        public static void AddOrder<T>(this QueryExpression query, Expression<Func<T, object>> attributeName, OrderType orderType) where T : Entity
        {
            query.AddOrder(ProperyExpression.GetName(attributeName), orderType);
        }

        /// <summary>
        /// Adds the specified link to the query expression setting the entity name to link
        /// to, the attribute name to link from and the attribute name to link to.</summary>
        /// <typeparam name="TFrom">Type of the entity to link from</typeparam>
        /// <typeparam name="TTo">Type of the entity to link to</typeparam>
        /// <param name="linkFromAttributeName">The property expressions containing the name of the attribute to link from.</param>
        /// <param name="linkToAttributeName">The property expressions containing the name of the attribute to link to.</param>
        public static LinkEntity AddLink<TFrom, TTo>(
            this QueryExpression query,
            Expression<Func<TFrom, object>> linkFromAttributeName,
            Expression<Func<TTo, object>> linkToAttributeName)
            where TFrom : Entity
            where TTo : Entity
        {
            return query.AddLink(typeof(TTo).Name.ToLower(),
                ProperyExpression.GetName(linkFromAttributeName),
                ProperyExpression.GetName(linkToAttributeName));
        }

        /// <summary>
        /// Adds the specified link to the query expression setting the entity name to link
        /// to, the attribute name to link from and the attribute name to link to.</summary>
        /// <typeparam name="TFrom">Type of the entity to link from</typeparam>
        /// <typeparam name="TTo">Type of the entity to link to</typeparam>
        /// <param name="linkFromAttributeName">The property expressions containing the name of the attribute to link from.</param>
        /// <param name="linkToAttributeName">The property expressions containing the name of the attribute to link to.</param>
        /// <param name="joinOperator">The join operator.</param>
        public static LinkEntity AddLink<TFrom, TTo>(
            this QueryExpression query,
            Expression<Func<TFrom, object>> linkFromAttributeName,
            Expression<Func<TTo, object>> linkToAttributeName, JoinOperator joinOperator)
            where TFrom : Entity
            where TTo : Entity
        {
            return query.AddLink(typeof(TTo).Name.ToLower(),
                ProperyExpression.GetName(linkFromAttributeName),
                ProperyExpression.GetName(linkToAttributeName),
                joinOperator);
        }
    }
}

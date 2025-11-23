using D365Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk.Query
{
    public sealed class QueryByAttribute<T> where T : Entity
    {
        /// <summary>
        /// Gets the set of attributes selected in the query and values to look for when the query is executed
        /// </summary>
        public Dictionary<Expression<Func<T, object>>, object> AttributeValues { get; private set; } = new Dictionary<Expression<Func<T, object>>, object>();

        /// <summary>
        /// Gets or sets the number of pages and the number of entity instances per page
        /// returned from the query
        /// </summary>
        public PagingInfo PageInfo { get; set; } = new PagingInfo();

        /// <summary>
        /// Gets or sets the column set
        /// </summary>
        public ColumnSet ColumnSet { get; set; } = new ColumnSet();

        /// <summary>
        /// Gets the order in which the entity instances are returned from the query
        /// </summary>
        public List<OrderExpression> Orders { get; private set; } = new List<OrderExpression>();

        /// <summary>
        /// Gets or sets the number of rows to be returned
        /// 
        /// When specified, this limits the number rows returned in a query result set to
        /// the specified number of rows.
        ///
        /// A query can contain either Microsoft.Xrm.Sdk.Query.QueryByAttribute.PageInfo
        /// or Microsoft.Xrm.Sdk.Query.QueryByAttribute.TopCount property values. If both
        /// are specified, an error will be thrown
        /// </summary>
        public int? TopCount { get; set; }

        /// <summary>
        /// Adds an order to the orders collection
        /// </summary>
        /// <param name="attributeName">The logical name of the attribute</param>
        /// <param name="orderType">The order for that attribute</param>
        public void AddOrder(Expression<Func<T, object>> attributeName, OrderType orderType)
        {
            Orders.Add(new OrderExpression<T>(attributeName, orderType));
        }

        /// <summary>
        /// Adds an attribute value to the attributes collection
        /// </summary>
        /// <param name="attributeName">The logical name of the attribute</param>
        /// <param name="value">The attribute value</param>
        public void AddAttributeValue(Expression<Func<T, object>> attributeName, object value)
        {
            this.AttributeValues.Add(attributeName, value);
        }

        public static implicit operator QueryByAttribute(QueryByAttribute<T> q)
        {
            if (q is null) return null;

            var queryByAttribute = new QueryByAttribute()
            {
                EntityName = LogicalName.GetName<T>(),
                ColumnSet = q.ColumnSet,
                TopCount = q.TopCount,
                PageInfo = q.PageInfo,
            };
            queryByAttribute.Attributes.AddRange(LogicalName.GetNames(q.AttributeValues.Keys));
            queryByAttribute.Values.AddRange(q.AttributeValues.Values);
            queryByAttribute.Orders.AddRange(q.Orders);

            return queryByAttribute;
        }
    }
}
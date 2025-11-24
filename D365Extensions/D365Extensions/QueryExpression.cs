using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk.Query
{
    public class QueryExpression<T> where T : Entity
    {
        /// <summary>
        /// Gets or sets the number of rows to be returned.
        ///
        /// When specified, this limits the number rows returned in a query result set to
        /// the specified number of rows.
        ///
        /// A query can contain either Microsoft.Xrm.Sdk.Query.QueryExpression.PageInfo or
        /// Microsoft.Xrm.Sdk.Query.QueryExpression.TopCount property values. If both are
        /// specified, an error will be thrown.
        /// </summary>
        public int? TopCount { get; set; }

        /// <summary>
        /// Gets or sets the number of pages and the number of entity instances per page
        /// returned from the query.
        ///
        /// A query can contain either Microsoft.Xrm.Sdk.Query.QueryExpression.PageInfo or
        /// Microsoft.Xrm.Sdk.Query.QueryExpression.TopCount property values. If both are
        /// specified, an error will be thrown.
        /// </summary>
        public PagingInfo PageInfo { get; set; } = new PagingInfo();

        /// <summary>
        /// Gets or sets the columns to include.
        /// </summary>
        public ColumnSet ColumnSet { get; set; } = new ColumnSet();

        /// <summary>
        /// Gets or sets the complex condition and logical filter expressions that filter
        /// </summary>
        public FilterExpression Criteria { get; set; } = new FilterExpression();

        /// <summary>
        ///  Gets or sets whether the results of the query contain duplicate entity instances.
        /// </summary>
        public bool Distinct { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates that no shared locks are issued against the
        /// data that would prohibit other transactions from modifying the data in the records
        /// returned from the query.
        ///
        /// The benefit of setting NoLock to true is that it allows you to keep the system
        /// from issuing locks against the entities in your queries; this increases concurrency
        /// and performance because the database engine does not have to maintain the shared
        /// locks involved. The downside is that, because no locks are issued against the
        /// records being read, some "dirty” or uncommitted data could potentially be read.
        /// A "dirty" read is one in which the data being read is involved in a transaction
        /// from another connection. If that transaction rolls back its work, the data read
        /// from the query using NoLock will have read uncommitted data. This type of read
        /// makes processing inconsistent and can lead to problems.
        /// </summary>
        public bool NoLock { get; set; }

        /// <summary>
        /// Gets a collection of the links between multiple entity types.
        /// </summary>
        public List<LinkEntity> LinkEntities { get; private set; } = new List<LinkEntity>();

        /// <summary>
        /// Gets the order in which the entity instances are returned from the query.
        /// </summary>
        public List<OrderExpression> Orders { get; private set; } = new List<OrderExpression>();

        public static implicit operator QueryExpression(QueryExpression<T> q)
        {
            if (q is null) return null;

            var queryExpression = new QueryExpression()
            {
                EntityName = LogicalName.GetName<T>(),
                TopCount = q.TopCount,
                PageInfo = q.PageInfo,
                ColumnSet = q.ColumnSet,
                Criteria = q.Criteria,
                Distinct = q.Distinct,
                NoLock = q.NoLock,
            };
            queryExpression.LinkEntities.AddRange(q.LinkEntities);
            queryExpression.Orders.AddRange(q.Orders);

            return queryExpression;
        }

        /// <summary>
        /// Adds the specified order expression to the query expression.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <param name="orderType">The order type.</param>
        public void AddOrder(Expression<Func<T, object>> attributeName, OrderType orderType)
        {
            Orders.Add(new OrderExpression(LogicalName.GetName(attributeName), orderType));
        }

        /// <summary>
        /// Adds the specified link to the query expression setting the entity name to link
        /// to, the attribute name to link from and the attribute name to link to.
        /// </summary>
        /// <typeparam name="TFrom">Type of the entity to link from</typeparam>
        /// <typeparam name="TTo">Type of the entity to link to</typeparam>
        /// <param name="linkFromAttributeName">The property expressions containing the name of the attribute to link from.</param>
        /// <param name="linkToAttributeName">The property expressions containing the name of the attribute to link to.</param>
        public LinkEntity AddLink<TFrom, TTo>(
            Expression<Func<TFrom, object>> linkFromAttributeName,
            Expression<Func<TTo, object>> linkToAttributeName)
            where TFrom : Entity
            where TTo : Entity
        {
            return AddLink(linkFromAttributeName, linkToAttributeName, JoinOperator.Inner);
        }

        /// <summary>
        /// Adds the specified link to the query expression setting the entity name to link
        /// to, the attribute name to link from and the attribute name to link to.
        /// </summary>
        /// <typeparam name="TFrom">Type of the entity to link from</typeparam>
        /// <typeparam name="TTo">Type of the entity to link to</typeparam>
        /// <param name="linkFromAttributeName">The property expressions containing the name of the attribute to link from.</param>
        /// <param name="linkToAttributeName">The property expressions containing the name of the attribute to link to.</param>
        /// <param name="joinOperator">The join operator.</param>
        public LinkEntity AddLink<TFrom, TTo>(
            Expression<Func<TFrom, object>> linkFromAttributeName,
            Expression<Func<TTo, object>> linkToAttributeName,
            JoinOperator joinOperator)
            where TFrom : Entity
            where TTo : Entity
        {
            var linkEntity = new LinkEntity()
            {
                LinkFromAttributeName = LogicalName.GetName(linkFromAttributeName),
                LinkFromEntityName = LogicalName.GetName<TFrom>(),
                LinkToAttributeName = LogicalName.GetName(linkToAttributeName),
                LinkToEntityName = LogicalName.GetName<TTo>(),
                JoinOperator = joinOperator,
            };

            LinkEntities.Add(linkEntity);

            return linkEntity;
        }
    }
}

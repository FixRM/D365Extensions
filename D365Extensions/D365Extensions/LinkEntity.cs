﻿using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Microsoft.Xrm.Sdk.Query
{
    /// <summary>
    /// Strongly typed version of the LinkEntity<TFrom, TTo> class.
    /// </summary>
    public sealed class LinkEntity<TFrom, TTo>
        where TFrom : Entity
        where TTo : Entity
    {
        /// <summary>
        /// Initializes a new instance of the LinkEntity class.
        /// </summary>
        public LinkEntity() : this(null, null, JoinOperator.Inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the LinkEntity class setting the required properties.
        /// </summary>
        /// <param name="linkFromAttributeName">The name of the attribute to link from.</param>
        /// <param name="linkToAttributeName">The name of the attribute to link to.</param>
        /// <param name="joinOperator">The join operator.</param>
        public LinkEntity(
          Expression<Func<TFrom, object>> linkFromAttributeName,
          Expression<Func<TTo, object>> linkToAttributeName,
          JoinOperator joinOperator)
        {
            this.LinkFromAttributeName = linkFromAttributeName;
            this.LinkToAttributeName = linkToAttributeName;
            this.JoinOperator = joinOperator;
            this.Columns = new ColumnSet();
            this.LinkCriteria = new FilterExpression();
            this.Orders = new List<OrderExpression>();
            this.LinkEntities = new List<LinkEntity>();
        }

        /// <summary>
        /// Gets or sets the property expressions containing the name of the attribute of the entity that you are linking from.
        /// </summary>
        public Expression<Func<TFrom, object>> LinkFromAttributeName { get; set; }

        /// <summary>
        /// Gets or sets the property expressions containing the name of the attribute of the entity that you are linking to.
        /// </summary>
        public Expression<Func<TTo, object>> LinkToAttributeName { get; set; }

        /// <summary>
        /// Gets or sets the join operator.
        /// </summary>
        public JoinOperator JoinOperator { get; set; }

        /// <summary>
        /// Gets or sets the alias for the entity.
        /// </summary>
        public string EntityAlias { get; set; }

        /// <summary>
        /// Gets or sets the column set.
        /// </summary>
        public ColumnSet Columns { get; set; }

        /// <summary>
        /// Gets or sets the complex condition and logical filter expressions that filter the results of the query.
        /// </summary>
        public FilterExpression LinkCriteria { get; set; }

        /// <summary>
        /// Gets the links between multiple entity types.
        /// </summary>
        public List<LinkEntity> LinkEntities { get; }

        /// <summary>
        /// Gets the orders for results of the query.
        /// </summary>
        public List<OrderExpression> Orders { get; }

        /// <summary>
        /// Converts LinkEntity<TFrom, TTo> to LinkEntity
        /// </summary>
        public static implicit operator LinkEntity(LinkEntity<TFrom, TTo> t)
        {
            if (t == null) return null;

            LinkEntity linkEntity = new LinkEntity()
            {
                Columns = t.Columns,
                EntityAlias = t.EntityAlias,
                JoinOperator = t.JoinOperator,
                LinkCriteria = t.LinkCriteria,
                LinkFromEntityName = LogicalName.GetName<TFrom>(),
                LinkFromAttributeName = LogicalName.GetName(t.LinkFromAttributeName),
                LinkToEntityName = LogicalName.GetName<TTo>(),
                LinkToAttributeName = LogicalName.GetName(t.LinkToAttributeName),
            };
            linkEntity.LinkEntities.AddRange(t.LinkEntities);
            linkEntity.Orders.AddRange(t.Orders);

            return linkEntity;
        }
    }
}

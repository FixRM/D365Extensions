using D365Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk.Query
{
    /// <summary>
    /// Extensions for LinkEntity
    /// </summary>
    public static class LinkEntityExtensions
    {
        /// <summary>
        /// Adds a link, setting the link to entity name, the link from attribute name and
        /// the link to attribute name.
        /// </summary>
        /// <typeparam name="TFrom">Type of the entity to link from</typeparam>
        /// <typeparam name="TTo">Type of the entity to link to</typeparam>
        /// <param name="linkToEntityName">The name of the entity to link to.</param>
        /// <param name="linkFromAttributeName">The property expressions containing the name of the attribute to link from.</param>
        /// <param name="linkToAttributeName">The property expressions containing the name of the attribute to link to.</param>
        /// <param name="joinOperator">The join operator.</param>
        /// <returns>The link entity that was created.</returns>
        public static LinkEntity AddLink<TFrom, TTo>(
            this LinkEntity link,
            string linkToEntityName,
            Expression<Func<TFrom, object>> linkFromAttributeName,
            Expression<Func<TTo, object>> linkToAttributeName,
            JoinOperator joinOperator)
        {
            return link.AddLink(
                linkToEntityName,
                ProperyExpression.GetName<TFrom>(linkFromAttributeName),
                ProperyExpression.GetName<TTo>(linkToAttributeName),
                joinOperator);
        }

        /// <summary>
        /// Adds a link, setting the link to entity name, the link from attribute name and
        /// the link to attribute name.
        /// </summary>
        /// <typeparam name="TFrom">Type of the entity to link from</typeparam>
        /// <typeparam name="TTo">Type of the entity to link to</typeparam>
        /// <param name="linkToEntityName">The name of the entity to link to.</param>
        /// <param name="linkFromAttributeName">The property expressions containing the name of the attribute to link from.</param>
        /// <param name="linkToAttributeName">The property expressions containing the name of the attribute to link to.</param>
        /// <returns>The link entity that was created.</returns>
        public static LinkEntity AddLink<TFrom, TTo>(
            this LinkEntity link,
            string linkToEntityName,
            Expression<Func<TFrom, object>> linkFromAttributeName,
            Expression<Func<TTo, object>> linkToAttributeName)
        {
            return link.AddLink(
                linkToEntityName,
                ProperyExpression.GetName<TFrom>(linkFromAttributeName),
                ProperyExpression.GetName<TTo>(linkToAttributeName));
        }
    }
}

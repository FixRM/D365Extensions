using D365Extensions;
using System;
using System.Linq.Expressions;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Generic version if AliasedValue class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class AliasedValue<T> where T : Entity
    {
        public Expression<Func<T, object>> AttributeName { get; }

        public object Value { get; }

        public AliasedValue(Expression<Func<T, object>> attributeName, object value)
        {
            AttributeName = attributeName;
            Value = value;
        }

        public static implicit operator AliasedValue(AliasedValue<T> t)
        {
            if (t is null) return null;

            return new AliasedValue(
                entityLogicalName: LogicalName.GetName<T>(),
                attributeLogicalName: LogicalName.GetName(t.AttributeName),
                value: t.Value);
        }
    }
}
using D365Extensions;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace D365Extensions
{
    /// <summary>
    /// Helper class for reading property names from lambda expressions
    /// </summary>
    public static class ProperyExpression
    {
        public static bool UseReflection { get; set; } = false;

        public static List<string> GetNames<T>(params Expression<Func<T, object>>[] expressions)
        {
            return expressions
                .Select(e => GetName(e))
                .ToList();
        }

        public static string GetName<T>(Expression<Func<T, object>> expression)
        {
            return GetName(expression?.Body);
        }

        static string GetName(Expression expression)
        {
            if (expression == null) return null;

            // Property, field of method returning value type
            if (expression is UnaryExpression unaryExpression)
            {
                expression = unaryExpression.Operand;
            }

            // Reference type property or field
            if (expression is MemberExpression memberExpession)
            {
                MemberInfo member = memberExpession.Member;

                if (UseReflection)
                {
                    var logicalName = member.GetCustomAttributes<AttributeLogicalNameAttribute>(false)
                        .FirstOrDefault();
                    if (logicalName != null)
                    {
                        return logicalName.LogicalName;
                    }
                    else throw new ArgumentException($"Property {member.Name} has no AttributeLogicalName attribute");
                }

                return member.Name.ToLowerInvariant();
            }

            throw CheckParam.InvalidExpression(nameof(expression));
        }
    }
}

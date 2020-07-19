using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace D365Extensions
{
    /// <summary>
    /// Helper class for reading property names from lambda expressions
    /// </summary>
    public static class ProperyExpression
    {
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
            if (expression == null)
            {
                return null;
            }

            // Property, field of method returning value type
            if (expression is UnaryExpression unaryExpression)
            {
                expression = unaryExpression.Operand;
            }

            // Reference type property or field
            if (expression is MemberExpression memberExpession)
            {
                return memberExpession.Member.Name.ToLower();
            }

            throw CheckParam.InvalidExpression(nameof(expression));
        }
    }
}

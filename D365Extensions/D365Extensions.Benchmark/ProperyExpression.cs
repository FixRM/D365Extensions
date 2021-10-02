using D365Extensions;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Concurrent;
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

        public static string GetNameR<T>(Expression<Func<T, object>> expression)
        {
            return GetNameR(expression?.Body);
        }

        public static string GetNameC<T>(Expression<Func<T, object>> expression)
        {
            return GetNameC(expression?.Body);
        }

        static ConcurrentDictionary<Expression, string> expressionChache = new ConcurrentDictionary<Expression, string>();

        public static string GetNameC2<T>(Expression<Func<T, object>> expression)
        {
            expressionChache.TryGetValue(expression, out string logicalName);

            if (logicalName == null)
            {
                logicalName = GetNameC2(expression?.Body);
                expressionChache.TryAdd(expression, logicalName);
            }

            return logicalName;
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

                return member.Name.ToLowerInvariant();
            }

            throw new ArgumentException(nameof(expression));
        }

        static string GetNameR(Expression expression)
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

                return member.GetCustomAttribute<AttributeLogicalNameAttribute>(false).LogicalName;
            }

            throw new ArgumentException(nameof(expression));
}

        static ConcurrentDictionary<MemberInfo, string> memberChache = new ConcurrentDictionary<MemberInfo, string>();

        static string GetNameC(Expression expression)
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

                memberChache.TryGetValue(member, out string logicalName);

                if (logicalName == null)
                {
                    logicalName = member.GetCustomAttribute<AttributeLogicalNameAttribute>(false).LogicalName;

                    memberChache.TryAdd(member, logicalName);
                }

                return logicalName;
            }

            throw new ArgumentException(nameof(expression));
        }

        static string GetNameC2(Expression expression)
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

                return member.GetCustomAttribute<AttributeLogicalNameAttribute>(false).LogicalName;
            }

            throw new ArgumentException(nameof(expression));
        }
    }
}

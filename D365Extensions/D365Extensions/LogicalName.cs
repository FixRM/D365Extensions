using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
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
    internal static class LogicalName
    {
        internal static IEnumerable<string> GetNames<T>(IEnumerable<Expression<Func<T, object>>> expressions)
        {
            foreach (var expression in expressions)
            {
                if (expression.Body is NewExpression newExpressionBody)
                {
                    for (int j = 0; j < newExpressionBody.Members.Count; j++)
                    {
                        yield return GetName<T>(newExpressionBody.Members[j], newExpressionBody.Arguments[j], isAnonymous: true);
                    }
                }
                else
                {
                    yield return GetName(expression);
                }
            }
        }

        internal static string GetName<T>(Expression<Func<T, object>> lambda)
        {
            if (lambda == null) return null;

            Expression expression = lambda.Body;

            // Property, field of method returning value type
            if (expression is UnaryExpression unaryExpression)
            {
                expression = unaryExpression.Operand;
            }

            // Reference type property or field
            if (expression is MemberExpression memberExpression)
            {
                MemberInfo member = memberExpression.Member;

                return GetName<T>(member, expression);
            }

            throw CheckParam.InvalidExpression(nameof(expression), lambda.ToString());
        }

        static ConcurrentDictionary<MemberInfo, string> memberCache = new ConcurrentDictionary<MemberInfo, string>();

        internal static string GetName<T>(MemberInfo member, Expression expression, bool isAnonymous = false)
        {
            CheckParam.CheckForNull(member, nameof(member));

            if (isAnonymous || typeof(Entity).IsAssignableFrom(member.DeclaringType))
            {
                // (Guid) Id attribute is declared in Entity class and is overridden in child EB-classes
                // For some reason, lambda is assigned with MemberInfo of Entity instead of inheritor
                if (member.DeclaringType != typeof(T))
                {
                    member = typeof(T).GetMember(member.Name).SingleOrDefault();
                }
            }
            else throw CheckParam.InvalidExpression(nameof(expression), expression.ToString());

            if (!memberCache.TryGetValue(member, out string logicalName))
            {
                logicalName = member.GetCustomAttribute<AttributeLogicalNameAttribute>()?.LogicalName
                    // fallback if attribute not provided
                    ?? member.Name.ToLowerInvariant();

                memberCache.TryAdd(member, logicalName);
            }

            return logicalName;
        }

        static ConcurrentDictionary<Type, string> typeCache = new ConcurrentDictionary<Type, string>();

        internal static string GetName<T>() where T : Entity
        {
            var type = typeof(T);

            if (!typeCache.TryGetValue(type, out string logicalName))
            {
                logicalName = type.GetCustomAttribute<EntityLogicalNameAttribute>()?.LogicalName
                    // fallback if attribute not provided
                    ?? type.Name.ToLowerInvariant();

                typeCache.TryAdd(type, logicalName);
            }

            return logicalName;
        }
    }
}
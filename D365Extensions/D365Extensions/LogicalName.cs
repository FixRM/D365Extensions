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
        internal static string[] GetNames<T>(params Expression<Func<T, object>>[] expressions)
        {
            var names = new string[expressions.Length];

            for (int i = 0; i < names.Length; i++)
            {
                names[i] = GetName(expressions[i]);
            }

            return names;
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
            if (expression is MemberExpression memberExpession)
            {
                MemberInfo member = memberExpession.Member;

                // (Guid) Id attribute is declared in Entity class and is overridden in child EB-classes
                // For some reason, lambda is assigned with MemberInfo of Entity instead of inheritor
                if (member.DeclaringType != typeof(T) && member.DeclaringType.IsAssignableFrom(typeof(Entity)))
                    member = typeof(T).GetMember(member.Name).SingleOrDefault();

                return GetName(member);
            }

            throw CheckParam.InvalidExpression(nameof(expression));
        }

        static ConcurrentDictionary<MemberInfo, string> memberCache = new ConcurrentDictionary<MemberInfo, string>();

        internal static string GetName(MemberInfo member)
        {
            CheckParam.CheckForNull(member, nameof(member));

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
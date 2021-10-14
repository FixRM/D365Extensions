using D365Extensions;
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
    public static class LogicalName
    {
        static ConcurrentDictionary<MemberInfo, string> memberChache = new ConcurrentDictionary<MemberInfo, string>();

        public static List<string> GetNames<T>(params Expression<Func<T, object>>[] expressions)
        {
            return expressions
                .Select(e => GetName(e))
                .ToList();
        }

        public static string GetName<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null) return null;

            return GetName(expression.Body);
        }

        static string GetName(Expression expression)
        {
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
                    logicalName = member.GetCustomAttribute<AttributeLogicalNameAttribute>()?.LogicalName
                        // fallback if attribute not provided
                        ?? member.Name.ToLowerInvariant();

                    memberChache.TryAdd(member, logicalName);
                }

                return logicalName;
            }

            throw CheckParam.InvalidExpression(nameof(expression));
        }

        static ConcurrentDictionary<Type, string> typeChache = new ConcurrentDictionary<Type, string>();

        public static string GetName<T>() where T : Entity
        {
            var type = typeof(T);

            typeChache.TryGetValue(type, out string logicalName);
            if (logicalName == null)
            {
                logicalName = type.GetCustomAttribute<EntityLogicalNameAttribute>()?.LogicalName
                    // fallback if attribute not provided
                    ?? type.Name.ToLowerInvariant();

                typeChache.TryAdd(type, logicalName);
            }

            return logicalName;
        }
    }
}

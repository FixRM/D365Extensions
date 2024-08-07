﻿using Microsoft.Xrm.Sdk;
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

        internal static string GetName<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null) return null;

            return GetName(expression.Body);
        }

        internal static string GetName(Expression expression)
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

                return GetName(member);
            }

            throw CheckParam.InvalidExpression(nameof(expression));
        }

        static ConcurrentDictionary<MemberInfo, string> memberChache = new ConcurrentDictionary<MemberInfo, string>();

        internal static string GetName(MemberInfo member)
        {
            if (!memberChache.TryGetValue(member, out string logicalName))
            {
                logicalName = member.GetCustomAttribute<AttributeLogicalNameAttribute>()?.LogicalName
                    // fallback if attribute not provided
                    ?? member.Name.ToLowerInvariant();

                memberChache.TryAdd(member, logicalName);
            }

            return logicalName;
        }

        static ConcurrentDictionary<Type, string> typeChache = new ConcurrentDictionary<Type, string>();

        internal static string GetName<T>() where T : Entity
        {
            var type = typeof(T);

            if (!typeChache.TryGetValue(type, out string logicalName))
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
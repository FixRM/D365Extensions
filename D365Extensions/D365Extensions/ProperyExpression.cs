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
    // This is experimental implementation
    // Members are not set to be [TreadStatic] as it can cause unexpected behaviour
    // for applications with complex thread logic, for example USD client
    //
    // On the other hand, assembly level attributes will not work as well as
    // Assembly.GetExecutingAssembly() will not work in sandoxed plugins
    // Please, let me know if you know better way to set this
    public static class D365ExtensionsSettings
    {
        public static bool UseReflection = true;
    }

    /// <summary>
    /// Helper class for reading property names from lambda expressions
    /// </summary>
    public static class ProperyExpression
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

                memberChache.TryGetValue(member, out string logicalName);
                if (logicalName == null)
                {
                    if (D365ExtensionsSettings.UseReflection)
                    {
                        logicalName = member.GetCustomAttribute<AttributeLogicalNameAttribute>().LogicalName;
                    }
                    else
                    {
                        logicalName = member.Name.ToLowerInvariant();
                    }

                    memberChache.TryAdd(member, logicalName);
                }

                return logicalName;
            }

            throw CheckParam.InvalidExpression(nameof(expression));
        }
    }
}

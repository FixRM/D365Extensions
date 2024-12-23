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

                // Hotfix: (Guid) Id attribute is declared in Entity class and is overriden in child EB-classes
                // For some reason, lamda is assigned with MemberInfo of Entity instead of inheritor
                // TODO: this fix is limited to single case of "Id" attribute, as it is not obvious if we need
                // more generic solution. Why does one need to override any other attribute schema name in
                // inherited class?

                if (member.Name == "Id") 
                    member = typeof(T).GetMember(member.Name).SingleOrDefault();

                return GetName(member);
            }

            throw CheckParam.InvalidExpression(nameof(expression));
        }

        static ConcurrentDictionary<MemberInfo, string> memberChache = new ConcurrentDictionary<MemberInfo, string>();

        internal static string GetName(MemberInfo member)
        {
            CheckParam.CheckForNull(member, nameof(member));

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
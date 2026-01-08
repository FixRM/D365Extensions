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
        internal static IEnumerable<string> GetNames<T>(IEnumerable<Expression<Func<T, object>>> lambdas)
        {
            foreach (var lambda in lambdas)
            {
                //It is ok to take nulls as OOB ColumnSet accept null column names
                if (lambda is null) yield return null;

                if (lambda.Body is NewExpression newExpressionBody)
                {
                    for (int i = 0; i < newExpressionBody.Members.Count; i++)
                    {
                        yield return GetName<T>(newExpressionBody.Arguments[i]);
                    }
                }
                else
                {
                    yield return GetName<T>(lambda.Body);
                }
            }
        }

        internal static string GetName<T>(Expression<Func<T, object>> lambda)
        {
            //It is ok to take nulls as all target name fields are nullable
            if (lambda == null) return null;

            return GetName<T>(lambda.Body);
        }

        static ConcurrentDictionary<MemberInfo, string> memberCache = new ConcurrentDictionary<MemberInfo, string>();
        
        private static string GetName<T>(Expression expression)
        {
            // Property, field of method returning value type
            if (expression is UnaryExpression unaryExpression)
                expression = unaryExpression.Operand;

            // Reference type property or field
            if (expression is not MemberExpression memberExpression)
                throw CheckParam.InvalidExpression(nameof(expression), expression.ToString());

                MemberInfo member = memberExpression.Member;

            // e.g.: a => a.PrimaryContact.Id instead of a => a.PrimaryContact
            if (!typeof(Entity).IsAssignableFrom(member.DeclaringType))
                throw CheckParam.InvalidExpression(nameof(expression), expression.ToString());
            
                    // (Guid) Id attribute is declared in Entity class and is overridden in child EB-classes
                    // For some reason, lambda is assigned with MemberInfo of Entity instead of inheritor
            // Also we support anonymous types where member-expressions come from their original type
                    if (member.DeclaringType != typeof(T))
                    {
                        member = typeof(T).GetMember(member.Name).SingleOrDefault();
                    }

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
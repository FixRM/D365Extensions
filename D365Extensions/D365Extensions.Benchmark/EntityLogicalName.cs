using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Xrm.Sdk.Client;
using System.Collections.Concurrent;

namespace D365Extensions
{
    public static class EntityLogicalName
    {
        static ConcurrentDictionary<Type, string> typeChache = new ConcurrentDictionary<Type, string>();

        static ConcurrentDictionary<Type, string> typeChache2 = new ConcurrentDictionary<Type, string>();

        public static string GetName<T>() where T : Entity
        {
            return typeof(T).Name.ToLowerInvariant();
        }

        public static string GetNameC<T>() where T : Entity
        {
            var type = typeof(T);

            typeChache.TryGetValue(type, out string logicalName);
            if (logicalName == null)
            {
                logicalName = type.Name.ToLowerInvariant();

                typeChache.TryAdd(type, logicalName);
            }

            return logicalName;
        }

        public static string GetNameR<T>() where T : Entity
        {
            return typeof(T).GetCustomAttribute<EntityLogicalNameAttribute>().LogicalName;
        }

        public static string GetNameRC<T>() where T : Entity
        {
            var type = typeof(T);

            typeChache2.TryGetValue(type, out string logicalName);
            if (logicalName == null)
            {
                logicalName = type.GetCustomAttribute<EntityLogicalNameAttribute>().LogicalName;

                typeChache2.TryAdd(type, logicalName);
            }

            return logicalName;
        }
    }
}

﻿using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk
{
    public static class DataCollectionExtensions
    {
        public static T GetValue<T>(this DataCollection<string, object> collection, string key)
        {
            if (collection.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            else
            {
                return default;
            }
        }
    }
}

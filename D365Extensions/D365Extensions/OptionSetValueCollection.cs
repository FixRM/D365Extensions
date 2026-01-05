#if NET462 || NET8_0

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Strongly typed version of the OptionSetValueCollection class
    /// </summary>
    public sealed class OptionSetValueCollection<T> : List<OptionSetValue<T>> where T : Enum
    {
        public OptionSetValueCollection()
        {
        }

        public OptionSetValueCollection(IList<OptionSetValue<T>> values) : base(values)
        {
        }

        public static implicit operator OptionSetValueCollection(OptionSetValueCollection<T> t)
        {
            if (t == null) return null;

            return new OptionSetValueCollection([.. t.Select(o => (OptionSetValue)o)]);
        }
    }
}

#endif
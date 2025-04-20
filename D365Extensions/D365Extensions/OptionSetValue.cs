using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Strongly typed version of the OptionSetValue class
    /// </summary>
    public class OptionSetValue<T> where T : Enum
    {
        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        public T Value { get; set; }

        public OptionSetValue()
        {
        }

        public OptionSetValue(T value)
        {
            Value = value; 
        }

        public static implicit operator OptionSetValue(OptionSetValue<T> t)
        {
            if (t == null) return null;

            return new OptionSetValue((int) (object) t.Value);
        }
    }
}

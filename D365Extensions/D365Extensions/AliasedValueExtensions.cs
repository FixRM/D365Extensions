using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk
{
    public static class AliasedValueExtensions
    {
        /// <summary>
        /// Checks if AliasedValue contains primary key attribute of linked entity 
        /// </summary>
        /// <returns>Id of linked entity</returns>
        public static bool IsPrimaryKey(this AliasedValue aliasedValue)
        {
            return aliasedValue.AttributeLogicalName == "activityid" ||
                aliasedValue.AttributeLogicalName.ConsistOf(aliasedValue.EntityLogicalName, "id");
        }

        /// <summary>
        /// Gets the value of AliasedValue
        /// </summary>
        public static T GetValue<T>(this AliasedValue aliasedValue)
        {
            return (T) aliasedValue.Value;
        }

        // to avoid string allocations
        private static bool ConsistOf(this string value, string part1, string part2)
        {
            if (value.Length != part1.Length + part2.Length)
                return false;

            return value.StartsWith(part1) && value.EndsWith(part2);
        }
    }
}
using Microsoft.Xrm.Sdk;

namespace D365Extensions
{
    public static class OptionSetValueExtensions
    {
        /// <summary>
        /// Returns OptionSetValue string representation
        /// </summary>
        /// <returns></returns>
        public static string ToTraceString(this OptionSetValue optionSet)
        {
            return $"OptionSetValue {{ Value = {optionSet.Value} }}";
        }
    }
}

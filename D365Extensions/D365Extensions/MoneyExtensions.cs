using Microsoft.Xrm.Sdk;

namespace D365Extensions
{
    public static class MoneyExtensions
    {
        /// <summary>
        /// Returns Money string representation
        /// </summary>
        /// <returns></returns>
        public static string ToTraceString(this Money money)
        {
            return $"Money {{ Value = {money.Value} }}";
        }
    }
}

using System;

namespace D365Extensions
{
    /// <summary>
    /// Helper class for throwing argument exceptions 
    /// </summary>
    internal static class CheckParam
    {
        internal static void CheckForNull(Object parameter, String name)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        internal static void OutOfRange(String name)
        {
            throw new ArgumentOutOfRangeException(name);
        }

        internal static ArgumentException InvalidExpression(string name)
        {
            return new ArgumentException("Invalid expression", name);
        }
    }
}
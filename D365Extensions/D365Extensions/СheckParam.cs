using System;

namespace D365Extensions
{
    /// <summary>
    /// Helper class for throwing argument exceptions 
    /// </summary>
    internal static class CheckParam
    {
        internal static void CheckForNull(object parameter, string name)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        internal static void BiggerThanOne(int parameter, string name)
        {
            if (parameter < 1)
            {
                throw new ArgumentOutOfRangeException(name);
            }
        }

        internal static ArgumentException InvalidExpression(string name)
        {
            return new ArgumentException("Invalid expression", name);
        }
    }
}
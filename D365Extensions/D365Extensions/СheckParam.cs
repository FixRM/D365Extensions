using System;

namespace D365Extensions
{
    /// <summary>
    /// Helper class for throwing argument exceptions 
    /// </summary>
    public static class CheckParam
    {
        public static void CheckForNull(Object parameter, String name)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void OutOfRange(String name)
        {
            throw new ArgumentOutOfRangeException(name);
        }

        public static ArgumentException InvalidExpression(string name)
        {
            return new ArgumentException("Invalid expression", name);
        }
    }
}
using System;

namespace D365Extensions
{
    /// <summary>
    /// Helper class for throwing argument exceptions 
    /// </summary>
    public static class СheckParam
    {
        public static void СheckForNull(Object parameter, String name)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
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
    }
}
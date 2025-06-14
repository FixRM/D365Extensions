using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Xml.Linq;

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

        internal const string InvalidExpressionMessage = "Invalid expression '{0}'";

        internal static ArgumentException InvalidExpression(string name, string expression)
        {
            return new ArgumentException(string.Format(InvalidExpressionMessage, expression), name);
        }

        internal const string InvalidPageNumberMessage = @"Query page number is bigger than one. It may mean that you are trying to get second iterator from IEnumerable<Entity> returned from RetrieveMultiple extension overload. For instance you may be using FirstOrDefault(Predicate<T>) or similar in a loop. It is not supported because this method is yielding results WITHOUT allocating each page in memory. Most likely it will lead to wrong results as second iterator will start from the same page there first one has stopped. Reseting page counter is either not an option as this will result in querying same data from the system multiple times. If you need iterate over this data multiple times you are responsible for allocation it in memory yourself.";

        internal static void CheckPageNumber(QueryBase query)
        {
            if (query.GetPageNumber() > 0)
            {
                throw new ArgumentException(InvalidPageNumberMessage);
            }
        }

        internal static void CheckPageNumber(FetchExpression fetch, XDocument document)
        {
            if (fetch.GetPageNumber(document) > 0)
            {
                throw new ArgumentException(InvalidPageNumberMessage);
            }
        }

        internal const string InvalidCollectionMessage = "This method supports only ActivityParty collections";

        internal static void IsActivityPartyCollection(EntityCollection collection)
        {
            if (collection.EntityName != "activityparty")
                throw new ArgumentException(InvalidCollectionMessage);
        }
    }
}
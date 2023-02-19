using D365Extensions;
using System.Collections.Generic;

namespace Microsoft.Xrm.Sdk
{
    internal static class OrganizationRequestCollectionEnumerator
    {
        internal static IEnumerable<OrganizationRequestCollection> Chunk(this IEnumerable<OrganizationRequest> source, int size)
        {
            CheckParam.CheckForNull(source, nameof(source));

            if (size < 1)
            {
                CheckParam.OutOfRange(nameof(size));
            }

            return ChunkIterator(source, size);
        }

        private static IEnumerable<OrganizationRequestCollection> ChunkIterator(IEnumerable<OrganizationRequest> source, int size)
        {
            using (IEnumerator<OrganizationRequest> e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    OrganizationRequestCollection requests = new OrganizationRequestCollection();
                    requests.Add(e.Current);

                    for (int i = 1; i < size; i++)
                    {
                        if (!e.MoveNext())
                        {
                            yield return requests;
                            yield break;
                        }

                        requests.Add(e.Current);
                    }

                    yield return requests;
                }
            }
        }
    }
}
using D365Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk;

public static class EntityCollectionExtensions
{
    /// <summary>
    /// Checks if ActivityParty list contains email address
    /// </summary>
    /// <param name="addressUsed">Email address to look for</param>
    /// <returns></returns>
    public static bool ContainsAddress(this EntityCollection collection, string addressUsed)
    {
        CheckParam.CheckForNull(addressUsed, nameof(addressUsed));
        CheckParam.IsActivityPartyCollection(collection);

        return collection.Entities.Any(e => addressUsed.Equals(GetEmail(e), StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Returns members of ActivityParty list whose email address refers to required domain
    /// </summary>
    /// <param name="addressUsedDomain">Email address domain to look for</param>
    /// <returns></returns>
    public static EntityCollection GetPartiesInDomain(this EntityCollection collection, string addressUsedDomain)
    {
        CheckParam.CheckForNull(addressUsedDomain, nameof(addressUsedDomain));
        CheckParam.IsActivityPartyCollection(collection);

        var partiesFromDomain = collection.Entities.Where(e => EmailEndsWith(e, addressUsedDomain));

        var ec = new EntityCollection();
        ec.EntityName = "activityparty";
        ec.Entities.AddRange(partiesFromDomain);

        return ec;
    }

    /// <summary>
    /// Returns members of ActivityParty list whose email address DON'T refers to required domain
    /// </summary>
    /// <param name="addressUsedDomain">Email address domain to look for</param>
    /// <returns></returns>
    public static EntityCollection GetPartiesNotInDomain(this EntityCollection collection, string addressUsedDomain)
    {
        CheckParam.CheckForNull(addressUsedDomain, nameof(addressUsedDomain));
        CheckParam.IsActivityPartyCollection(collection);

        var partiesNotFromDomain = collection.Entities.Where(e => !EmailEndsWith(e, addressUsedDomain));

        var ec = new EntityCollection();
        ec.EntityName = "activityparty";
        ec.Entities.AddRange(partiesNotFromDomain);

        return ec;
    }

    private static bool EmailEndsWith(Entity party, string value)
    {
        var email = GetEmail(party);

        return email?.EndsWith(value, StringComparison.OrdinalIgnoreCase) == true;
    }

    private static string GetEmail(Entity party)
    {
        return party.GetAttributeValue<string>("addressused");
    }
}
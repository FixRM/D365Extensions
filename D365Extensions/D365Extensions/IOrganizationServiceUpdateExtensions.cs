using D365Extensions;
using Microsoft.Xrm.Sdk.Messages;
using System;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Extensions for IOrganizationService.Update
    /// </summary>
    public static partial class IOrganizationServiceExtensions
    {
        /// <summary>
        /// Update method override. Accept ConcurrencyBehavior parameter
        /// </summary>
        /// <param name="reference">Entity to delete</param>
        public static void Update(this IOrganizationService service, Entity entity, ConcurrencyBehavior concurrencyBehavior)
        {
            CheckParam.CheckForNull(entity, nameof(entity));

            service.Execute(new UpdateRequest()
            {
                Target = entity,
                ConcurrencyBehavior = concurrencyBehavior
            });
        }
    }
}

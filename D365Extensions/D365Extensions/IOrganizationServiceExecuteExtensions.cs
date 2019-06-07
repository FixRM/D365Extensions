using D365Extensions;
using Microsoft.Xrm.Sdk.Messages;
using System;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Extensions for IOrganizationService.Execute
    /// </summary>
    public static partial class IOrganizationServiceExtensions
    {
        /// <summary>
        /// Execute method override. Returns OrganizationResponse as the specified type
        /// </summary>
        public static T Execute<T>(this IOrganizationService service, OrganizationRequest request) where T : OrganizationResponse
        {
            CheckParam.CheckForNull(request, nameof(request));

            return service.Execute(request) as T;
        }

        /// <summary>
        /// A shortcut for Upsert message. There is much more
        /// </summary>
        public static EntityReference Upsert(this IOrganizationService service, Entity entity)
        {
            UpsertResponse response = service.Execute<UpsertResponse>(new UpsertRequest()
            {
                Target = entity
            });

            return response.Target;
        }
    }
}
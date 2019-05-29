using D365Extensions;

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
    }
}
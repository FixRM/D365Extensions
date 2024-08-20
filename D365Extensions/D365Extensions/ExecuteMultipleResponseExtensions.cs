using Microsoft.Xrm.Sdk.Messages;

namespace Microsoft.Xrm.Sdk
{
    internal static class ExecuteMultipleResponseExtensions
    {
        //Case 1: bulk create => may need id of each record
        //Case 2: bulk update or delete => need to know only failures with record id's
        //Case 3: ???

        // Experimental
        public static OrganizationRequestCollection GetRequests(this ExecuteMultipleResponse response)
        {
            return response.Results.GetValue<OrganizationRequestCollection>("Requests");
        }

        // Experimental
        public static OrganizationRequest GetRequest(this ExecuteMultipleResponse response, ExecuteMultipleResponseItem item)
        {
            return response.GetRequests()?[item.RequestIndex];
        }
    }
}
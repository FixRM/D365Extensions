using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Xrm.Sdk
{
    public static class ExecuteMultipleResponseExtensions
    {
        //two methods above work in pair with IOrganizationServiceExtensions.Execute(IEnumerable<OrganizationRequest> requests) extensions
        #region Execute partners

        [Obsolete]
        public static OrganizationRequestCollection GetRequests(this ExecuteMultipleResponse response)
        {
            return response.Results.GetValue<OrganizationRequestCollection>("Requests");
        }

        [Obsolete]
        public static OrganizationRequest GetRequest(this ExecuteMultipleResponse response, ExecuteMultipleResponseItem item)
        {
            return response.GetRequests()?[item.RequestIndex];
        }

        #endregion

        /// <summary>
        /// Returns collection of faulted ExecuteMultipleResponseItems
        /// </summary>
        public static ExecuteMultipleResponseItemCollection GetFaultedResponses(this ExecuteMultipleResponse response)
        {
            var results = new ExecuteMultipleResponseItemCollection();
            results.AddRange(response.GetFaultedResponsesInternal());

            return results;
        }

        internal static IEnumerable<ExecuteMultipleResponseItem> GetFaultedResponsesInternal(this ExecuteMultipleResponse response)
        {
            return response.Responses.Where(r => r.IsFaulted());
        }

        /// <summary>
        /// Throws AggregateException that contains faults from related ExecuteMultipleResponseItems
        /// </summary>
        /// <exception cref="AggregateException"></exception>
        public static void ThrowIfFaulted(this ExecuteMultipleResponse response)
        {
            if (response.IsFaulted)
            {
                var faulted = response.GetFaultedResponsesInternal();

                throw new AggregateException(faulted.Select(f => f.GetFaultException()));
            }
        }
    }
}
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <summary>
        /// Returns collection of faulted ExecuteMultipleResponseItems
        /// </summary>
        public static ExecuteMultipleResponseItemCollection GetFaultedResponses(this ExecuteMultipleResponse response)
        {
            var results = new ExecuteMultipleResponseItemCollection();
            results.AddRange(response.GetFaultedResponsesInternal());

            return results;
        }

        private static IEnumerable<ExecuteMultipleResponseItem> GetFaultedResponsesInternal(this ExecuteMultipleResponse response)
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
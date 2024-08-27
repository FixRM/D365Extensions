using D365Extensions;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;

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
        /// A shortcut for Upsert message. There is much more messages to create shortcut for, but this one is only useful for daily CRUD operations
        /// </summary>
        public static EntityReference Upsert(this IOrganizationService service, Entity entity)
        {
            CheckParam.CheckForNull(entity, nameof(entity));

            UpsertResponse response = service.Execute<UpsertResponse>(new UpsertRequest()
            {
                Target = entity
            });

            return response.Target;
        }

        /// <summary>
        /// Execute batch of requests using ExecuteMultipleRequest while taking care of batch size
        /// 
        /// NEVER use this extension as well as ExecuteMultipleRequest itself in Plugin code
        /// https://learn.microsoft.com/en-us/power-apps/developer/data-platform/best-practices/business-logic/avoid-batch-requests-plugin
        /// </summary>
        /// <param name="requests">The collection of message requests to execute</param>
        /// <param name="settings">The settings that define whether execution should continue if an
        //  error occurs and if responses for each message request processed are to be returned</param>
        /// <param name="batchSize">The number of requests to be sent in each ExecuteMultipleRequest.</param>
        [Obsolete("Use IEnumerable<ExecuteMultipleOperationResponse> Execute(IEnumerable<OrganizationRequest> requests,int batchSize, ExecuteMultipleSettings settings, Action<ExecuteMultipleResponse> callback")]
        public static IEnumerable<ExecuteMultipleResponse> Execute(this IOrganizationService service,
            IEnumerable<OrganizationRequest> requests,
            int batchSize = 100,
            ExecuteMultipleSettings settings = null)
        {
            CheckParam.CheckForNull(requests, nameof(requests));

            settings = settings ?? new ExecuteMultipleSettings();

            foreach (var collection in requests.Chunk(batchSize))
            {
                var response = service.Execute(new ExecuteMultipleRequest()
                {
                    Requests = collection,
                    Settings = settings
                }) as ExecuteMultipleResponse;

                // Experimental
                response["Requests"] = collection;

                yield return response;

                if (!settings.ContinueOnError && response.IsFaulted)
                {
                    yield break;
                }
            }
        }

        /// Execute batch of requests using ExecuteMultipleRequest while taking care of batch size and enumerating results
        /// 
        /// NEVER use this extension as well as ExecuteMultipleRequest itself in Plugin code
        /// https://learn.microsoft.com/en-us/power-apps/developer/data-platform/best-practices/business-logic/avoid-batch-requests-plugin
        /// </summary>
        /// <param name="requests">The collection of message requests to execute</param>
        /// <param name="settings">The settings that define whether execution should continue if an
        //  error occurs and if responses for each message request processed are to be returned</param>
        /// <param name="batchSize">The number of requests to be sent in each ExecuteMultipleRequest.</param>
        /// <param name="callback">Optional function to be called for each ExecuteMultipleResponse</param>
        /// <returns></returns>
        public static IEnumerable<ExecuteMultipleOperationResponse> Execute(this IOrganizationService service,
            IEnumerable<OrganizationRequest> requests,
            int batchSize = 100,
            ExecuteMultipleSettings settings = null,
            Action<OrganizationRequestCollection, ExecuteMultipleResponse> callback = null)
        {
            CheckParam.CheckForNull(requests, nameof(requests));

            settings = settings ?? new ExecuteMultipleSettings();

            foreach (OrganizationRequestCollection collection in requests.Chunk(batchSize))
            {
                var eMultipleResponse = service.Execute(new ExecuteMultipleRequest()
                {
                    Requests = collection,
                    Settings = settings
                }) as ExecuteMultipleResponse;

                callback?.Invoke(collection, eMultipleResponse);
                
                for (int i = 0; i < eMultipleResponse.Responses.Count; i++)
                {
                    ExecuteMultipleResponseItem item = eMultipleResponse.Responses[i];
                    var request = collection[item.RequestIndex];

                    yield return new ExecuteMultipleOperationResponse(item, request);
                }

                // if ContinueOnError is false, then faulted response is last one in response collection
                // we don't need to check each response inside a loop
                if (!settings.ContinueOnError && eMultipleResponse.IsFaulted)
                    yield break;
            }
        }
    }
}
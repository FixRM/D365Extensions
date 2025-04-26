using D365Extensions;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

        /// <summary>
        /// Runs query and executes action for each retrieved Entity using ExecuteMultipleRequest
        /// 
        /// NEVER use this extension as well as ExecuteMultipleRequest itself in Plugin code
        /// https://learn.microsoft.com/en-us/power-apps/developer/data-platform/best-practices/business-logic/avoid-batch-requests-plugin
        /// </summary>
        /// <param name="query">Query to retrieve entities</param>
        /// <param name="toRequest">Delegate that converts Entity to OrganizationRequest that should be executed. Return null if no action is required</param>
        /// <param name="settings">Settings to be passed with ExecuteMultipleRequest</param>
        /// <param name="batchSize">The number of requests to be sent in each ExecuteMultipleRequest</param>
        /// <param name="onResponse">Optional function to be called for each ExecuteMultipleOperationResponse</param>
        /// <param name="onProgress">Optional function to report progress. Progress is calculated as processed * 100.0 / queried items</param>
        /// <param name="cancellationToken">Optional cancellation token that can be used to cancel bulk processing</param>
        public static void Execute(this IOrganizationService service,
            QueryBase query,
            Func<Entity, OrganizationRequest> toRequest,
            ExecuteMultipleSettings settings = null,
            int batchSize = 100,
            Action<ExecuteMultipleOperationResponse> onResponse = null,
            Action<ExecuteMultipleProgress> onProgress = null,
            CancellationToken cancellationToken = default)
        {
            CheckParam.CheckForNull(query, nameof(query));
            CheckParam.CheckForNull(toRequest, nameof(toRequest));

            uint queried = 0;
            uint processed = 0;
            uint skipped = 0;
            uint errors = 0;

            void reportProgress() => onProgress?.Invoke(new ExecuteMultipleProgress(queried, processed, skipped, errors));

            bool notSkipped(OrganizationRequest req)
            {
                if (req == null) skipped++;

                return req != null;
            }

            Action<EntityCollection> queryProgress = null;
            Action<OrganizationRequestCollection, ExecuteMultipleResponse> batchProgress = null;

            if (onProgress != null)
            {
                queryProgress = (ec) =>
                {
                    queried += (uint)ec.Entities.Count;
                    reportProgress();
                };

                batchProgress = (coll, emr) =>
                {
                    if (emr.IsFaulted)
                    {
                        if (settings.ReturnResponses == false)
                        {
                            errors += (uint)emr.Responses.Count;
                        }
                        else // less optimal fallback
                        {
                            errors += (uint)emr.GetFaultedResponsesInternal().Count();
                        }
                    }

                    processed += (uint)coll.Count;
                    reportProgress();
                };
            }

            var entities = service.RetrieveMultiple(query, queryProgress);

            var requests = entities.Select(toRequest).Where(notSkipped);

            var eMultipleOperationResponses = service.Execute(requests, batchSize, settings, batchProgress);

            foreach (var eMultipleOperationResponse in eMultipleOperationResponses)
            {
                onResponse?.Invoke(eMultipleOperationResponse);

                // It is not async method so we, probably, should silently exit instead of throwing the error
                if (cancellationToken.IsCancellationRequested)
                    break;
            }

            //to update if last requests where skipped
            reportProgress();
        }

        /// <summary>
        /// Updates only changed attributes of the record.
        /// 
        /// WARNING: This method will execute Retrieve operation before performing Update.
        /// Consider using Entity.RemoveUnchanged if you already have as-is and to-be entity instances
        /// </summary
        /// <param name="entity">An entity instance that has one or more properties set to be updated in the record</param>
        public static void UpdateChanged(this IOrganizationService service, Entity entity)
        {
            var columnSet = entity.ToColumnSet();

            // to retrieve via Id or alternative keys
            var existingEntity = service.Retrieve(entity.ToEntityReference(withKeys: true), columnSet);

            //TODO: optimize
            var entityToUpdate = entity.Clone();
            entityToUpdate.RemoveUnchanged(existingEntity);

            service.Update(entityToUpdate);
        }
    }
}
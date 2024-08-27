namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Combines ExecuteMultipleResponseItem with its original OrganizationRequest
    /// </summary>
    public sealed class ExecuteMultipleOperationResponse
    {
        internal ExecuteMultipleResponseItem ExecuteMultipleResponseItem { get; }

        public OrganizationRequest Request { get; }

        public OrganizationResponse Response => ExecuteMultipleResponseItem.Response;

        public OrganizationServiceFault Fault => ExecuteMultipleResponseItem.Fault;

        public bool IsFaulted => Fault != null;

        public ExecuteMultipleOperationResponse(ExecuteMultipleResponseItem executeMultipleResponseItem, OrganizationRequest request)
        {
            ExecuteMultipleResponseItem = executeMultipleResponseItem;
            Request = request;
        }

        public T GetRequest<T>() where T : OrganizationRequest
        {
            return Request as T;
        }

        public T GetResponse<T>() where T : OrganizationResponse
        {
            return Response as T;
        }

        public void ThrowIfFaulted()
        {
            ExecuteMultipleResponseItem.ThrowIfFaulted();
        }
    }
}
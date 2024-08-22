using System.ServiceModel;

namespace Microsoft.Xrm.Sdk
{
    public static class ExecuteMultipleResponseItemExtensions
    {
        /// <summary>
        /// Checks if this ExecuteMultipleResponseItem if contains a fault
        /// </summary>
        public static bool IsFaulted(this ExecuteMultipleResponseItem item)
        {
            return item.Fault != null;
        }

        /// <summary>
        /// Creates FaultException<OrganizationServiceFault> from this ExecuteMultipleResponseItem`s fault
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static FaultException<OrganizationServiceFault> GetFaultException(this ExecuteMultipleResponseItem item)
        {
            return new FaultException<OrganizationServiceFault>(item.Fault);
        }

        /// <summary>
        /// Throws a FaultException<OrganizationServiceFault> if this ExecuteMultipleResponseItem if contains a fault
        /// </summary>
        /// <exception cref="FaultException<OrganizationServiceFault>"></exception>
        public static void ThrowIfFaulted(this ExecuteMultipleResponseItem item)
        {
            if (item.IsFaulted())
            {
                throw new FaultException<OrganizationServiceFault>(item.Fault);
            }
        }
    }
}
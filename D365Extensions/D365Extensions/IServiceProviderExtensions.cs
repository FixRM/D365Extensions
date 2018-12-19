using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Set of extension methods for Microsoft.Xrm.Sdk.IServiceProvider base class. Just shortcut methods to save you few lines of code during plugin development
    /// </summary>
    public static class IServiceProviderExtensions
    {
        /// <summary>
        /// Gets IPluginExecutionContext from service provider
        /// </summary>
        /// <returns></returns>
        public static IPluginExecutionContext GetPluginExecutionContext(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService(typeof(IPluginExecutionContext)) as IPluginExecutionContext;
        }

        /// <summary>
        /// Gets IOrganizationServiceFactory from service provider
        /// </summary>
        /// <returns></returns>
        public static IOrganizationServiceFactory GetOrganizationServiceFactory(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService(typeof(IOrganizationServiceFactory)) as IOrganizationServiceFactory;
        }

        /// <summary>
        /// Gets ITracingService from service provider
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static ITracingService GetTracingService(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService(typeof(ITracingService)) as ITracingService;
        }
    }
}

#if NET452
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Activities
{
    public static class CodeActivityContextExtensions
    {
        /// <summary>
        /// Gets IWorkflowContext extension from CodeActivityContext
        /// </summary>
        /// <returns></returns>
        public static IWorkflowContext GetWorkflowContext(this CodeActivityContext codeActivityContext)
        {
            return codeActivityContext.GetExtension<IWorkflowContext>();
        }

        /// <summary>
        /// Gets IOrganizationServiceFactory extension from CodeActivityContext
        /// </summary>
        /// <param name="codeActivityContext"></param>
        /// <returns></returns>
        public static IOrganizationServiceFactory GetOrganizationServiceFactory(this CodeActivityContext codeActivityContext)
        {

            return codeActivityContext.GetExtension<IOrganizationServiceFactory>();
        }

        /// <summary>
        /// Gets ITracingService extension from CodeActivityContext
        /// </summary>
        /// <param name="codeActivityContext"></param>
        /// <returns></returns>
        public static ITracingService GetTracingService(this CodeActivityContext codeActivityContext)
        {
            return codeActivityContext.GetExtension<ITracingService>();
        }
    }
}
#endif
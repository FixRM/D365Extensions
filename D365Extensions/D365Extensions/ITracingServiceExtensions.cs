using Microsoft.Xrm.Sdk;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions
{
    /// <summary>
    /// Set of extension methods for Microsoft.Xrm.Sdk.ITracingService base class.
    /// </summary>
    public static class ITracingServiceExtensions
    {
        /// <summary>
        /// Logs basic IPluginExecutionContext trace information such as MessageName, Stage, PrimaryEntityId, etc.
        /// </summary>
        /// <param name="context">IPluginExecutionContext instance</param>
        public static void TracePluginContext(this ITracingService tracingService, IPluginExecutionContext context)
        {
            tracingService.Trace($@"MessageName: ""{context.MessageName}""");
            tracingService.Trace($@"Stage: {context.Stage}");
            tracingService.Trace($@"PrimaryEntityId: ""{context.PrimaryEntityId:B}""");
            tracingService.Trace($@"PrimaryEntityName: ""{context.PrimaryEntityName}""");
            tracingService.Trace($@"UserId: ""{context.UserId:B}""");
            tracingService.Trace($@"InitiatingUserId: ""{context.InitiatingUserId:B}""");
            tracingService.Trace($@"Depth: {context.Depth}");
            tracingService.Trace($@"Mode: {context.Mode}");
        }

        /// <summary>
        /// Logs IPluginExecutionContext InputParameters content.
        /// </summary>
        /// <param name="context">IPluginExecutionContext instance</param>

        public static void TraceInputParameters(this ITracingService tracingService, IPluginExecutionContext context, string header = "InputParameters")
        {
            Trace(tracingService, context.InputParameters, header);
        }

        /// <summary>
        /// Logs IPluginExecutionContext OutputParameters content.
        /// </summary>
        /// <param name="context">IPluginExecutionContext instance</param>
        public static void TraceOutputParameters(this ITracingService tracingService, IPluginExecutionContext context, string header = "OutputParameters")
        {
            Trace(tracingService, context.OutputParameters, header);
        }

        /// <summary>
        /// Logs IPluginExecutionContext SharedVariables content.
        /// </summary>
        /// <param name="context">IPluginExecutionContext instance</param>
        public static void TraceSharedVariables(this ITracingService tracingService, IPluginExecutionContext context, string header = "SharedVariables")
        {
            Trace(tracingService, context.SharedVariables, header);
        }

        /// <summary>
        /// Logs IPluginExecutionContext PreEntityImages content.
        /// </summary>
        /// <param name="context">IPluginExecutionContext instance</param>
        public static void TracePreEntityImages(this ITracingService tracingService, IPluginExecutionContext context, string header = "PreEntityImages")
        {
            Trace(tracingService, context.PreEntityImages, header);
        }

        /// <summary>
        /// Logs IPluginExecutionContext PostEntityImages content.
        /// </summary>
        /// <param name="context">IPluginExecutionContext instance</param>
        public static void TracePostEntityImages(this ITracingService tracingService, IPluginExecutionContext context, string header = "PostEntityImages")
        {
            Trace(tracingService, context.PostEntityImages, header);
        }

        internal static void Trace(this ITracingService tracingService, ParameterCollection parameters, string header)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendDataCollection(parameters, header);

            tracingService.Trace(builder.ToString());
        }

        internal static void Trace(this ITracingService tracingService, EntityImageCollection images, string header)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendDataCollection(images, header);

            tracingService.Trace(builder.ToString());
        }
    }
}
using Microsoft.Xrm.Sdk;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace D365Extensions
{
    /// <summary>
    /// PluginExecutionTraceContext is a disposable wrapper around IPluginExecutionContext and ITracingService
    /// that simplifies tracing of plugin execution context properties in memory efficient way
    /// 
    /// You SHOULD notice that plugin execution context will be printed only at the and of plugin execution
    /// for performance reasons. If you modify plugin execution context during runtime, e.g. modify Target
    /// or add OutputParameters, then MODIFIED execution context will be printed. If you want to trace original
    /// values, you should trace them manually or using existing extension methods like TraceInputParameters
    /// </summary>
    public sealed class PluginExecutionTraceContext : ITracingService, IDisposable
    {
        public IPluginExecutionContext PluginExecutionContext { get; private set; }

        public PluginExecutionTraceContextSettings Settings { get; set; }

        // this one should be private to prevent user from using it directly
        internal ITracingService TracingService { get; private set; }

        private readonly StringBuilder builder = new StringBuilder();

        public PluginExecutionTraceContext(ITracingService tracingService, IPluginExecutionContext pluginContext, PluginExecutionTraceContextSettings settings = null)
        {
            CheckParam.CheckForNull(tracingService, nameof(tracingService));
            CheckParam.CheckForNull(pluginContext, nameof(pluginContext));

            this.PluginExecutionContext = pluginContext;
            this.TracingService = tracingService;
            this.Settings = settings ?? new PluginExecutionTraceContextSettings();
        }

        public void Dispose()
        {
            // by default we print trace only in case of error
            if (Settings.ErrorOnly && Marshal.GetExceptionCode() == 0)
                return;

            TracingService.TracePluginContext(PluginExecutionContext);

            if (PluginExecutionContext.InputParameters?.Count > 0 || Settings.ShowEmptyCollections)
                TracingService.TraceInputParameters(PluginExecutionContext);

            if (PluginExecutionContext.PreEntityImages?.Count > 0 || Settings.ShowEmptyCollections)
                TracingService.TracePreEntityImages(PluginExecutionContext);

            if (PluginExecutionContext.SharedVariables?.Count > 0 || Settings.ShowEmptyCollections)
                TracingService.TraceSharedVariables(PluginExecutionContext);

            if (PluginExecutionContext.OutputParameters?.Count > 0 || Settings.ShowEmptyCollections)
                TracingService.TraceOutputParameters(PluginExecutionContext);

            if (PluginExecutionContext.PostEntityImages?.Count > 0 || Settings.ShowEmptyCollections)
                TracingService.TracePostEntityImages(PluginExecutionContext);

            // once I've seen a bug in online version there ParentContext reference was pointing to current context
            if (PluginExecutionContext.ParentContext != null && Settings.IncludeParentContext && PluginExecutionContext != PluginExecutionContext.ParentContext)
            {
                TracingService.Trace("Parent context:");

                var parentContext = new PluginExecutionTraceContext(TracingService, PluginExecutionContext.ParentContext, Settings);
                parentContext.Dispose();
            }

            // pushing collected logs
            if (builder.Length > 0)
            {
                TracingService.Trace(builder.ToString());
            }
        }

        public void Trace(string format, params object[] args)
        {
            builder.AppendFormat(format, args);
            builder.AppendLine();
        }
    }
}

namespace D365Extensions
{
    public sealed class PluginExecutionTraceContextSettings
    {
        /// <summary>
        /// Print, empty collections, such as PostEntityImages, or OutputParameters even if they are empty. Default = false
        /// </summary>
        public bool ShowEmptyCollections { get; set; } = false;

        /// <summary>
        /// Print output only in case of error. Default = true
        /// You SHOULD notice that tracing code will be executed even if you turn off tracing in your D365 environment.
        /// Meanwhile, serializing plugin execution context that include EntityImages or InputParameters may be memory consuming operation.
        /// If this setting is set to "true" tracing will be skipped for plugins that there executed without errors.
        /// </summary>
        public bool ErrorOnly { get; set; } = true;

        /// <summary>
        /// Print ParentContext. Default = false
        /// You SHOULD notice that it is experimental feature. It may cause infinite recursion loop
        /// </summary>
        public bool IncludeParentContext { get; set; } = false;
    }
}

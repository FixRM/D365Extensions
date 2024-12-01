namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Contains progress information for long running data processing operations
    /// </summary>
    public record struct ExecuteMultipleProgress(uint Queried, uint Processed, uint Skipped, uint Errors)
    {
        public float Progress => Queried != 0 ? ((float)(Processed + Skipped)) / Queried * 100.0F : 100;

        public float ErrorRate => Processed != 0 ? ((float)Errors ) / Processed * 100.0F : 0;

        public float SkippedRate => Queried != 0 ? ((float)Skipped) / Queried * 100.0F : 0;
    }
}
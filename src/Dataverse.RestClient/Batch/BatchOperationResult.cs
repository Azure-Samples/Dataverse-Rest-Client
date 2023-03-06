namespace Dataverse.RestClient
{
    /// <summary>
    /// Result of single operation in the Batch Request.
    /// </summary>
    public class BatchOperationResult
    {
        /// <summary>
        /// Request ID
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Original Request that was sent.
        /// </summary>
        public HttpContent RequestContent { get; private set; }

        /// <summary>
        /// Response of the request. Null if the request is a change set.
        /// </summary>
        public MultipartSingleResponse? Response { get; private set; } = null;

        /// <summary>
        /// List of results of change set operations.
        /// </summary>
        public List<ChangeSetResult>? ChangeSetResults { get; private set; } = null;

        /// <summary>
        /// Indicates if the result is a change set.
        /// </summary>
        public bool IsChangeSet { get { return ChangeSetResults != null; } }

        public BatchOperationResult(
            int index, 
            HttpContent requestContent, 
            MultipartSingleResponse response)
        {
            this.Index = index;
            this.RequestContent = requestContent;
            this.Response = response;
        }

        public BatchOperationResult(
            int index,
            HttpContent requestContent,
            List<ChangeSetResult> changeSetResults)
        {
            this.Index = index;
            this.RequestContent = requestContent;
            this.ChangeSetResults = changeSetResults;
        }
    }
}

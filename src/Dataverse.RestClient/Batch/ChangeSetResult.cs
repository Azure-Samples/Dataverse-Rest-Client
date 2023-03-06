namespace Dataverse.RestClient
{
    /// <summary>
    /// Represents a result of a change set operation in batch request.
    /// </summary>
    public class ChangeSetResult
    {
        /// <summary>
        /// Request ID of the changeset.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Original Request that was sent.
        /// </summary>
        public HttpContent RequestContent { get; private set; }

        /// <summary>
        /// Response of the request.
        /// </summary>
        public MultipartSingleResponse? Response { get; private set; }

        public ChangeSetResult(
            int index,
            HttpContent requestContent,
            MultipartSingleResponse response)
        {
            Index = index;
            RequestContent = requestContent;
            Response = response;
        }
    }
}

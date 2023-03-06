namespace Dataverse.RestClient
{
    public class BatchOperationResult
    {
        public int Index { get; private set; }

        public HttpContent RequestContent { get; private set; }

        public MultipartSingleResponse? Response { get; private set; } = null;

        public List<ChangeSetResult>? ChangeSetResults { get; private set; } = null;

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

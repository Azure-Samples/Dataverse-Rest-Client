namespace Dataverse.RestClient
{

    public class ChangeSetResult
    {
        public int Index { get; private set; }

        public HttpContent RequestContent { get; private set; }

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

namespace Dataverse.RestClient
{
    using System;
    using System.Net.Http;

    public interface IChangeSet
    {
        int AddCreate(string entitySetName, string jsonData);
        int AddDelete(string entitySetName, Guid itemId);
        int AddHttpRequestMessage(HttpContent request);
        int AddUpdate(string entitySetName, Guid itemId, string jsonData);
        MultipartContent ToMultipartContent();
        Task<List<ChangeSetResult>> ProcessAsync(HttpContent content, CancellationToken cancellationToken);
    }
}
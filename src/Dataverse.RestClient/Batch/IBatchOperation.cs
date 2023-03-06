namespace Dataverse.RestClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IBatchOperation
    {
        Task<IEnumerable<BatchOperationResult>> ProcessAsync(CancellationToken cancellationToken = default);
        
        int AddContent(HttpContent content);

        int AddChangeSet(IChangeSet changeSet);

        int AddCreate(string entitySetName, string jsonData)
        {
            var message = CreateHttpMessageRequest(DataverseClient.BuildRequestUrl(entitySetName), "POST", jsonData);
            return AddContent(message.ToBatchHttpMessageContent());
        }

        int AddDelete(string entitySetName, Guid itemId)
        {
            var message = CreateHttpMessageRequest(DataverseClient.BuildRequestUrl(entitySetName, null, itemId), "DELETE");
            return AddContent(message.ToBatchHttpMessageContent());
        }

        int AddRead(string entitySetName, string? subEntitySetName = null, Guid? itemId = null, string? fetchXml = null, string? select = null, string? inlineCount = null, string? filter = null, int? top = null, string? expand = null, string? expandSelect = null)
        {
            var message = CreateHttpMessageRequest(DataverseClient.BuildRequestUrl(entitySetName, subEntitySetName, itemId, fetchXml, select, inlineCount, filter, null, expand, top, expandSelect), "GET");
            return AddContent(message.ToBatchHttpMessageContent());
        }

        int AddUpdate(string entitySetName, Guid itemId, string jsonData)
        {
            var message = CreateHttpMessageRequest(DataverseClient.BuildRequestUrl(entitySetName, null, itemId), "PATCH", jsonData);
            return AddContent(message.ToBatchHttpMessageContent());
        }

        HttpRequestMessage CreateHttpMessageRequest(string url, string method = "POST", string body = "{}");

        IChangeSet CreateChangeSet();
    }
}

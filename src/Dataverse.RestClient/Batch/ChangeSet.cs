namespace Dataverse.RestClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;

    public class ChangeSet: IChangeSet
    {
        private Dictionary<int, HttpContent> requests = new();
        private int requestsCount = -1;
        private readonly string baseAddress;

        internal ChangeSet(string baseAddress) 
        {
            this.baseAddress = baseAddress;
        }

        public int AddHttpRequestMessage(HttpContent request)
        {
            requestsCount++;
            request.Headers.Add("Content-ID", requestsCount.ToString());
            requests.Add(requestsCount, request);
            return requestsCount;
        }

        public int AddCreate(string entitySetName, string jsonData)
        {
            var message = CreateHttpMessageRequest(DataverseClient.BuildRequestUrl(entitySetName), "POST", jsonData);
            return AddHttpRequestMessage(message.ToBatchHttpMessageContent());
        }

        public int AddDelete(string entitySetName, Guid itemId)
        {
            var message = CreateHttpMessageRequest(DataverseClient.BuildRequestUrl(entitySetName, null, itemId), "DELETE");
            return AddHttpRequestMessage(message.ToBatchHttpMessageContent());
        }

        public int AddUpdate(string entitySetName, Guid itemId, string jsonData)
        {
            var message = CreateHttpMessageRequest(DataverseClient.BuildRequestUrl(entitySetName, null, itemId), "PATCH", jsonData);
            return AddHttpRequestMessage(message.ToBatchHttpMessageContent());
        }

        public MultipartContent ToMultipartContent()
        {
            var content = new MultipartContent("mixed", $"changeset_{Guid.NewGuid()}");

            foreach (var request in requests)
            {
                content.Add(request.Value);
            }

            return content;
        }

        internal HttpRequestMessage CreateHttpMessageRequest(string url, string method = "POST", string body = "{}") => new HttpRequestMessage(new HttpMethod(method), new Uri(this.baseAddress + url, UriKind.Absolute))
        {
            Version = new Version(1, 1),
            Content = JsonContent.Create(body)
        };

        public async Task<List<ChangeSetResult>> ProcessAsync(HttpContent content, CancellationToken cancellationToken)
        {
            var multipartResponse = await content.ReadAsMultipartAsync(cancellationToken);
            var result = new List<ChangeSetResult>();
            foreach (var (c ,i) in multipartResponse.Contents.Select((value, i) => (value, i)))
            {
                var mpResponse = await MultipartSingleResponse.Create(c, cancellationToken);
                result.Add(new ChangeSetResult(i, requests[i], mpResponse));

            }
            return result;
        }
    }
}

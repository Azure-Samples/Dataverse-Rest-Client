namespace Dataverse.RestClient
{
    using Dataverse.RestClient.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    public class MultipartSingleResponse
    {
        public HttpStatusCode StatusCode { get; }
        public EntityReference? EntityReference { get; }
        public JsonElement? ResponseBody { get; }

        private MultipartSingleResponse(HttpStatusCode statusCode, EntityReference? entityReference, JsonElement? responseBody)
        {
            StatusCode = statusCode;
            EntityReference = entityReference;
            ResponseBody = responseBody;
        }

        public static async Task<MultipartSingleResponse> Create(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken = default)
        {
            var statusCode = httpResponseMessage.StatusCode;
            var entityReference = GetEntityReference(httpResponseMessage.Headers);
            var responseBody = await GetResponseBodyAsync(httpResponseMessage.Content);

            return new(statusCode, entityReference, responseBody);
        }

        public static async Task<MultipartSingleResponse> Create(HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            httpContent.Headers.Remove("Content-Type");
            httpContent.Headers.Add("Content-Type", "application/http;msgtype=response");
            return await Create(await httpContent.ReadAsHttpResponseMessageAsync(cancellationToken), cancellationToken);
        }

        private static async Task<JsonElement?> GetResponseBodyAsync(HttpContent content)
        {
            var result = await content.ReadAsStringAsync();

            try
            {
                result = result.Substring(result.IndexOf("{"), result.Length - result.IndexOf("{"));
            }
            catch
            {
                return null;
            }

            using var jsonDoc = JsonDocument.Parse(result);
            return jsonDoc.RootElement.Clone();
        }

        private static EntityReference? GetEntityReference(HttpResponseHeaders headers)
        {
            if (headers.TryGetValues("OData-EntityId", out var odataLink))
            {
                return new(odataLink.FirstOrDefault()!);
            }
            return null;
        }
    }
}

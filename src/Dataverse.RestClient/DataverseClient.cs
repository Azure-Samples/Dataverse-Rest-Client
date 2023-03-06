using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Dataverse.RestClient.Test")]
namespace Dataverse.RestClient
{
    using Dataverse.RestClient.Model;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Reflection;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public partial class DataverseClient : IDataverseClient
    {
        private readonly HttpClient httpClient;
        private readonly string baseAddress;

        public DataverseClient(HttpClient httpClient, DataverseClientOptions options)
        {
            this.baseAddress = $"{options.DataverseBaseUrl}/api/data/v{options.Version}/";
            this.httpClient = ConfigureHttpClient(httpClient, options);
        }

        public virtual HttpClient ConfigureHttpClient(HttpClient httpClient, DataverseClientOptions options)
        {
            httpClient.BaseAddress = new Uri(this.baseAddress);
            httpClient.Timeout = TimeSpan.FromSeconds(options.TimeoutInSeconds);
            httpClient.DefaultRequestHeaders.Add("User-Agent", $"WebAPIService/{Assembly.GetExecutingAssembly().GetName().Version}");
            // Set default headers for all requests
            // See https://docs.microsoft.com/en-us/power-apps/developer/data-platform/webapi/compose-http-requests-handle-errors#http-headers
            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("If-None-Match", "null");
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        public Task<JsonArrayResponse> ListAsync(
            string entitySetName,
            string? subEntitySetName = null,
            Guid? itemId = null,
            string? fetchXml = null,
            string? select = null,
            string? inlineCount = null,
            string? filter = null,
            int? top = null,
            string? orderby = null,
            string? expand = null,
            string? expandSelect = null,
            bool withAnnotations = false,
            Func<JsonElement, object?, JsonElement>? convert = null,
            object? eventArgs = null,
            JsonArrayResponse? previousResponse = null,
            CancellationToken cancellationToken = default)
        {
            return this.GetJsonArrayResponse(BuildRequestUrl(entitySetName,
                                                             subEntitySetName,
                                                             itemId,
                                                             fetchXml,
                                                             select,
                                                             inlineCount,
                                                             filter,
                                                             orderby,
                                                             expand,
                                                             top,
                                                             expandSelect), convert, withAnnotations, previousResponse, eventArgs, cancellationToken: cancellationToken);
        }

        public Task<JsonArrayResponse<TData>> ListAsync<TData>(
            string entitySetName,
            string? subEntitySetName = null,
            Guid? itemId = null,
            string? fetchXml = null,
            string? select = null,
            string? inlineCount = null,
            string? filter = null,
            int? top = null,
            string? orderby = null,
            string? expand = null,
            string? expandSelect = null,
            bool withAnnotations = false,
            Func<JsonElement, object?, TData>? convert = null,
            object? eventArgs = null,
            JsonArrayResponse<TData>? previousResponse = null,
            CancellationToken cancellationToken = default)
        {
            return this.GetJsonArrayResponse(BuildRequestUrl(entitySetName,
                                                             subEntitySetName,
                                                             itemId,
                                                             fetchXml,
                                                             select,
                                                             inlineCount,
                                                             filter,
                                                             orderby,
                                                             expand,
                                                             top,
                                                             expandSelect), convert, withAnnotations, previousResponse, eventArgs, cancellationToken: cancellationToken);

        }

        public Task<JsonArrayResponse<TData, TEventArgs>> ListAsync<TData, TEventArgs>(
            string entitySetName,
            string? subEntitySetName = null,
            Guid? itemId = null,
            string? fetchXml = null,
            string? select = null,
            string? inlineCount = null,
            string? filter = null,
            int? top = null,
            string? orderby = null,
            string? expand = null,
            string? expandSelect = null,
            bool withAnnotations = false,
            Func<JsonElement, TEventArgs?, TData>? convert = null,
            TEventArgs? eventArgs = default,
            JsonArrayResponse<TData, TEventArgs>? previousResponse = null,
            CancellationToken cancellationToken = default)
        {
            return this.GetJsonArrayResponse(BuildRequestUrl(entitySetName,
                                                             subEntitySetName,
                                                             itemId,
                                                             fetchXml,
                                                             select,
                                                             inlineCount,
                                                             filter,
                                                             orderby,
                                                             expand,
                                                             top,
                                                             expandSelect), convert, withAnnotations, previousResponse, eventArgs, cancellationToken: cancellationToken);

        }

        public Task<HttpResponseMessage> ListAsStreamAsync(
            string entitySetName,
            string? subEntitySetName = null,
            Guid? itemId = null,
            string? fetchXml = null,
            string? select = null,
            string? inlineCount = null,
            string? filter = null,
            int? top = null,
            string? orderby = null,
            string? expand = null,
            string? expandSelect = null,
            bool withAnnotations = false,
            CancellationToken cancellationToken = default)
        {
            return this.GetHttpResponseMessageWithoutContent(BuildRequestUrl(entitySetName,
                                                             subEntitySetName,
                                                             itemId,
                                                             fetchXml,
                                                             select,
                                                             inlineCount,
                                                             filter,
                                                             orderby,
                                                             expand,
                                                             top,
                                                             expandSelect), withAnnotations, cancellationToken: cancellationToken);

        }

        public Task<HttpResponseMessage> PatchAsync(
            string entitySetName,
            string jsonData,
            Guid itemId,
            string? subEntitySetName = null,
            bool withRepresentation = false,
            bool withAnnotations = false,
            CancellationToken cancellationToken = default)
        {
            return this.PatchAsync(BuildRequestUrl(entitySetName, itemId: itemId, subEntitySetName: subEntitySetName), jsonData, withRepresentation, withAnnotations, cancellationToken);
        }

        public Task<HttpResponseMessage> PatchAsync(
            string entitySetName,
            Stream data,
            Guid itemId,
            string? subEntitySetName = null,
            bool withRepresentation = false,
            bool withAnnotations = false,
            CancellationToken cancellationToken = default)
        {
            return this.PatchAsync(BuildRequestUrl(entitySetName, itemId: itemId, subEntitySetName: subEntitySetName), data, withRepresentation, withAnnotations, cancellationToken);
        }

        public async Task<T?> PatchAsync<T>(
            string entitySetName,
            string jsonData,
            Guid itemId,
            string? subEntitySetName = null,
            bool withAnnotations = false,
            Func<JsonElement, T>? convert = null,
            CancellationToken cancellationToken = default)
        {
            var responseMessage = await this.PatchAsync(
                entitySetName,
                jsonData,
                itemId,
                subEntitySetName,
                true,
                withAnnotations,
                cancellationToken);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                if (convert == null)
                {
                    return JsonSerializer.Deserialize<T>(result);
                }
                using var jsonDoc = JsonDocument.Parse(result);
                return convert(jsonDoc.RootElement.Clone());
            }

            throw await DataverseWebApiException.Parse(responseMessage);
        }

        public Task<HttpResponseMessage> PostAsync(
            string entitySetName,
            string jsonData,
            string? subEntitySetName = null,
            Guid? itemId = null,
            string? select = null,
            string? expand = null,
            bool withRepresentation = false,
            bool withAnnotations = false,
            CancellationToken cancellationToken = default)
        {
            return this.PostAsync(BuildRequestUrl(
                entitySetName,
                subEntitySetName: subEntitySetName,
                itemId: itemId,
                select: select,
                expand: expand), jsonData, withRepresentation, withAnnotations, cancellationToken);
        }

        public async Task<EntityReference> PostAsync(
            string entitySetName,
            string jsonData,
            string? subEntitySetName = null,
            Guid? itemId = null,
            CancellationToken cancellationToken = default)
        {
            var responseMessage = await this.PostAsync(
                entitySetName,
                jsonData,
                subEntitySetName,
                itemId,
                null,
                null,
                false,
                false,
                cancellationToken);

            if (responseMessage.IsSuccessStatusCode)
            {
                var odataLink = responseMessage.Headers.GetValues("OData-EntityId").FirstOrDefault();
                return new EntityReference(odataLink ?? "");
            }

            throw await DataverseWebApiException.Parse(responseMessage);
        } 

        public async Task<T?> PostAsync<T>(
            string entitySetName,
            string jsonData,
            string? subEntitySetName = null,
            Guid? itemId = null,
            string? select = null,
            string? expand = null,
            bool withAnnotations = false,
            Func<JsonElement, T>? convert = null,
            CancellationToken cancellationToken = default)
        {
            var responseMessage = await this.PostAsync(
                entitySetName,
                jsonData,
                subEntitySetName,
                itemId,
                select,
                expand,
                true,
                withAnnotations,
                cancellationToken);

            if (responseMessage.IsSuccessStatusCode)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                if (convert == null)
                {
                    return JsonSerializer.Deserialize<T>(result);
                }
                using var jsonDoc = JsonDocument.Parse(result);
                return convert(jsonDoc.RootElement.Clone());
            }

            throw await DataverseWebApiException.Parse(responseMessage);
        }

        public Task<HttpResponseMessage> DeleteAsync(string entitySetName, Guid? itemId, CancellationToken cancellationToken = default)
        {
            return this.DeleteAsync(BuildRequestUrl(entitySetName, itemId: itemId), cancellationToken);
        }

        public async Task<HttpResponseMessage> SendBatchAsync(string batchId, IEnumerable<HttpContent> contents, CancellationToken cancellationToken = default)
        {
            var content = new MultipartContent("mixed", $"batch_{batchId}");
            
            foreach (var item in contents)
            {
                content.Add(item);
            }

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("$batch", UriKind.Relative),
                Method = HttpMethod.Post,
                Content = content
            };

            var response = await this.httpClient.SendAsync(request, cancellationToken);
            return response;
        }

        public IBatchOperation CreateBatchOperation()
        {
            return new BatchOperation(this, this.baseAddress);
        }
    }
}

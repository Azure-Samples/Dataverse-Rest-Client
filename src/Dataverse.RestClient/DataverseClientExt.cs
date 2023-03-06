namespace Dataverse.RestClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Json;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public partial class DataverseClient
    {
        protected virtual async Task<JsonDocument> GetJsonResponse(
            string requestUrl,
            bool withAnnotations = false,
            bool usingFullLink = false,
            CancellationToken cancellationToken = default)
        {
            var responseMessage = await this.GetAsync(requestUrl, withAnnotations, usingFullLink, cancellationToken: cancellationToken);
            string response = await responseMessage.Content.ReadAsStringAsync();
            return JsonDocument.Parse(response);
        }

        private async Task<JsonArrayResponse> GetJsonArrayResponse(
            string requestUrl,
            Func<JsonElement, object?, JsonElement>? convert = null,
            bool withAnnotations = false,
            JsonArrayResponse? previousResponse = null,
            object? eventArgs = null,
            CancellationToken cancellationToken = default)
        {
            using var jsonResponse = await this.GetJsonResponse(previousResponse == null ? requestUrl : (string.IsNullOrEmpty(previousResponse.NextLink) ? requestUrl : previousResponse.NextLink), withAnnotations, previousResponse != null && !string.IsNullOrEmpty(previousResponse.NextLink), cancellationToken);
            return new JsonArrayResponse(jsonResponse, convert, eventArgs, previousResponse == null ? 1 : previousResponse.Page + 1);
        }

        private async Task<JsonArrayResponse<TData>> GetJsonArrayResponse<TData>(
            string requestUrl,
            Func<JsonElement, object?, TData>? convert,
            bool withAnnotations = false,
            JsonArrayResponse<TData>? previousResponse = null,
            object? eventArgs = null,
            CancellationToken cancellationToken = default)
        {
            using var jsonResponse = await this.GetJsonResponse(previousResponse == null ? requestUrl : (string.IsNullOrEmpty(previousResponse.NextLink) ? requestUrl : previousResponse.NextLink), withAnnotations, previousResponse != null && !string.IsNullOrEmpty(previousResponse.NextLink), cancellationToken);
            return new JsonArrayResponse<TData>(jsonResponse, convert, eventArgs, previousResponse == null ? 1 : previousResponse.Page + 1);
        }

        private async Task<JsonArrayResponse<TData, TEventArgs>> GetJsonArrayResponse<TData, TEventArgs>(
            string requestUrl,
            Func<JsonElement, TEventArgs?, TData>? convert,
            bool withAnnotations = false,
            JsonArrayResponse<TData, TEventArgs>? previousResponse = null,
            TEventArgs? eventArgs = default,
            CancellationToken cancellationToken = default)
        {
            using var jsonResponse = await this.GetJsonResponse(previousResponse == null ? requestUrl : (string.IsNullOrEmpty(previousResponse.NextLink) ? requestUrl : previousResponse.NextLink), withAnnotations, previousResponse != null && !string.IsNullOrEmpty(previousResponse.NextLink), cancellationToken);
            return new JsonArrayResponse<TData, TEventArgs>(jsonResponse, convert, eventArgs, previousResponse == null ? 1 : previousResponse.Page + 1);
        }

        private async Task<HttpResponseMessage> GetAsync(
            string requestUrl,
            bool withAnnotations = false,
            bool usingFullLink = false,
            HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
            CancellationToken cancellationToken = default)
        {
            HttpRequestMessage httpRequestMessage;
            if (usingFullLink)
                httpRequestMessage = this.CreateHttpRequestMessage(HttpMethod.Get, new Uri(requestUrl, UriKind.Absolute));
            else
                httpRequestMessage = this.CreateHttpRequestMessage(HttpMethod.Get, new Uri(requestUrl, UriKind.Relative));

            if (withAnnotations)
                httpRequestMessage.Headers.Add("Prefer", "odata.include-annotations=\"*\"");

            var responseMessage = await this.httpClient.SendAsync(httpRequestMessage, completionOption, cancellationToken);
            if (responseMessage.IsSuccessStatusCode)
                return responseMessage;

            throw await DataverseWebApiException.Parse(responseMessage);
        }

        private Task<HttpResponseMessage> GetHttpResponseMessageWithoutContent(
            string requestUrl,
            bool withAnnotations = false,
            bool usingFullLink = false,
            CancellationToken cancellationToken = default)
        {
            return this.GetAsync(requestUrl, withAnnotations, usingFullLink, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }

        private async Task<HttpResponseMessage> PostAsync(
            string requestUrl,
            string jsonData,
            bool withRepresentation,
            bool withAnnotations,
            CancellationToken cancellationToken = default)
        {
            var request = this.CreateHttpRequestMessage(HttpMethod.Post, new Uri(requestUrl, UriKind.RelativeOrAbsolute), JsonContent.Create(jsonData));
            this.AddRepresentationHeader(request, withRepresentation, withAnnotations);
            var responseMessage = await this.httpClient.SendAsync(request, cancellationToken);
            if (responseMessage.IsSuccessStatusCode)
                return responseMessage;

            throw await DataverseWebApiException.Parse(responseMessage);
        }

        private async Task<HttpResponseMessage> PatchAsync(
            string requestUrl,
            string jsonData,
            bool withRepresentation,
            bool withAnnotations,
            CancellationToken cancellationToken = default)
        {
            var request = this.CreateHttpRequestMessage(new HttpMethod("PATCH"), new Uri(requestUrl, UriKind.RelativeOrAbsolute), JsonContent.Create(jsonData));
            this.AddRepresentationHeader(request, withRepresentation, withAnnotations);
            var responseMessage = await this.httpClient.SendAsync(request, cancellationToken);
            if (responseMessage.IsSuccessStatusCode)
                return responseMessage;

            throw await DataverseWebApiException.Parse(responseMessage);
        }

        private async Task<HttpResponseMessage> PatchAsync(
            string requestUrl,
            Stream data,
            bool withRepresentation,
            bool withAnnotations,
            CancellationToken cancellationToken = default)
        {
            data.Position = 0;
            var request = this.CreateHttpRequestMessage(new HttpMethod("PATCH"), new Uri(requestUrl, UriKind.RelativeOrAbsolute), new StreamContent(data));
            this.AddRepresentationHeader(request, withRepresentation, withAnnotations);
            request.Content!.Headers.Remove("Content-Type");
            request.Content!.Headers.Add("Content-Type", "application/octet-stream");
            var responseMessage = await this.httpClient.SendAsync(request, cancellationToken);
            if (responseMessage.IsSuccessStatusCode)
                return responseMessage;

            throw await DataverseWebApiException.Parse(responseMessage);
        }

        private async Task<HttpResponseMessage> DeleteAsync(string requestUrl, CancellationToken cancellationToken = default)
        {
            var request = this.CreateHttpRequestMessage(HttpMethod.Delete, new Uri(requestUrl, UriKind.RelativeOrAbsolute));
            var responseMessage = await this.httpClient.SendAsync(request, cancellationToken);
            if (responseMessage.IsSuccessStatusCode)
                return responseMessage;

            throw await DataverseWebApiException.Parse(responseMessage);
        }

        private void AddRepresentationHeader(
            HttpRequestMessage request,
            bool withRepresentation,
            bool withAnnotations)
        {
            var headers = new List<string>();

            if (withAnnotations)
            {
                headers.Add("odata.include-annotations=*");
            }

            if (withRepresentation)
            {
                headers.Add("return=representation");
            }

            if (headers.Count > 0)
            {
                request.Headers.Add("Prefer", string.Join(',', headers));
            }
        }

        private HttpRequestMessage CreateHttpRequestMessage(
            HttpMethod httpMethod,
            Uri requestUri,
            HttpContent? httpContent = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, requestUri);
            if (httpContent != null)
                request.Content = httpContent;
            return request;
        }

        internal static string BuildRequestUrl(
            string entitySetName,
            string? subEntitySetName = null,
            Guid? itemId = null,
            string? fetchXml = null,
            string? select = null,
            string? inlineCount = null,
            string? filter = null,
            string? orderby = null,
            string? expand = null,
            int? top = null,
            string? expandSelect = null)
        {
            select = RemoveWhiteSpaces(select);
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(entitySetName ?? "");
            List<string> queryParameters = new();
            bool AppendQueryString(string format, string? parameter, bool escaped)
            {
                if (!string.IsNullOrEmpty(parameter))
                {
                    queryParameters.Add(string.Format(format, escaped ? Uri.EscapeDataString(parameter) : parameter));
                    return true;
                }

                return false;
            };
            urlBuilder.Append((!itemId.HasValue) ? string.Empty : ("(" + itemId.Value.ToCleanString() + ")"));
            urlBuilder.Append(string.IsNullOrEmpty(subEntitySetName) ? string.Empty : ("/" + subEntitySetName));
            AppendQueryString("fetchXml={0}", fetchXml, true);
            AppendQueryString("$select={0}", select, false);
            AppendQueryString("$inlinecount={0}", inlineCount, false);
            AppendQueryString("$orderby={0}", orderby, false);
            AppendQueryString("$filter={0}", filter, false);
            AppendQueryString("$top={0}", (!top.HasValue) ? string.Empty : top.ToString()!, false);
            if (!string.IsNullOrEmpty(expand))
            {
                if (!string.IsNullOrEmpty(expandSelect))
                {
                    expand = expand + "($select=" + expandSelect + ")";
                }

                queryParameters.Add("$expand=" + expand);
            }

            if (queryParameters.Count > 0)
            {
                urlBuilder.Append('?').Append(string.Join("&", queryParameters));
            }
            return urlBuilder.ToString();
        }

        private static string? RemoveWhiteSpaces(string? text) => !string.IsNullOrEmpty(text) ? text.Replace(" ", string.Empty) : text;
    }
}

namespace Dataverse.RestClient
{
    using System.Collections;
    using System.Text.Json;

    public class JsonArrayResponse<TData, TEventArgs> : IEnumerable<TData>, IEnumerable
    {
        protected const string ODATA_NEXT_LINK_FIELD = "@odata.nextLink";
        protected const string JSON_TOKEN_NAME_ERROR = "error";
        protected const string JSON_TOKEN_NAME_VALUE = "value";
        protected IEnumerable<TData> results = new List<TData>();

        public string NextLink { get; private set; }

        public int Page { get; private set; }

        public DataverseWebApiException? Exception { get; protected set; }

        public JsonArrayResponse(JsonDocument? response, int page = 1)
        {
            this.Page = page;
            this.NextLink = string.Empty;
            if (response?.RootElement.TryGetProperty(ODATA_NEXT_LINK_FIELD, out var nextLinkElement) == true)
            {
                this.NextLink = nextLinkElement.ToString();
            }
        }

        public JsonArrayResponse(
          JsonDocument? response = null,
          Func<JsonElement, TEventArgs?, TData>? convert = null,
          TEventArgs? eventArgs = default,
          int page = 1)
          : this(response, page)
        {
            if (response == null)
            {
                return;
            }
            var responseElement = response.RootElement.Clone();
            try
            {
                if (responseElement.TryGetProperty(JSON_TOKEN_NAME_ERROR, out var errorElement))
                {
                    this.Exception = new DataverseWebApiException(errorElement);
                }
                if (convert == null)
                {
                    return;
                }
                if (responseElement.TryGetProperty(JSON_TOKEN_NAME_VALUE, out var valueElement) 
                    && valueElement.ValueKind == JsonValueKind.Array)
                {
                    this.results = valueElement.EnumerateArray().Select(result => convert(result, eventArgs));
                }
                else
                {
                    this.results = new List<TData>()
                    {
                        convert(responseElement, eventArgs)
                    };
                }
            }
            catch
            {
            }
        }

        public void Clear()
        {
            this.results = new List<TData>();
            this.Exception = null;
        }

        public IEnumerator<TData> GetEnumerator() => this.results.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.results.GetEnumerator();
    }

    public class JsonArrayResponse<TData> : JsonArrayResponse<TData, object>, IEnumerable<TData>, IEnumerable
    {
        public JsonArrayResponse(JsonDocument response, int page = 1)
          : base(response, page)
        {
        }

        public JsonArrayResponse(
            JsonDocument? response = null,
            Func<JsonElement, object?, TData>? convert = null,
            object? eventArgs = null,
            int page = 1)
          : base(response, convert, eventArgs, page)
        {
        }
    }

    public class JsonArrayResponse : JsonArrayResponse<JsonElement>, IEnumerable<JsonElement>, IEnumerable
    {
        public JsonArrayResponse(
            JsonDocument? response = null,
            Func<JsonElement, object?, JsonElement>? convert = null,
            object? eventArgs = null,
            int page = 1)
          : base(response, convert ?? ((e, __) => e), eventArgs, page)
        {
        }
    }
}

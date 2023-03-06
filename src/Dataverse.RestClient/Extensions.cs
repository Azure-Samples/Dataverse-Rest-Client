namespace Dataverse.RestClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Json;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public static class Extensions
    {
        internal static string ToCleanString(this Guid guid) => guid.ToString()
                                                                    .Replace("{", string.Empty)
                                                                    .Replace("}", string.Empty)
                                                                    .Replace(" ", string.Empty);
        internal static bool IsJSON(this string jsonString)
        {
            try
            {
                JsonDocument.Parse(jsonString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static HttpMessageContent ToBatchHttpMessageContent(this HttpRequestMessage message)
        {
            var httpMessageContent = new HttpMessageContent(message);
            httpMessageContent.Headers.Remove("Content-Type");
            httpMessageContent.Headers.Add("Content-Type", "application/http");
            httpMessageContent.Headers.Add("Content-Transfer-Encoding", "binary");
            return httpMessageContent;
        }

        public static ResultType? DeserializeOrDefault<ResultType>(this JsonElement element, string fieldName, ResultType? defaultValue = default)
        {
            if (!element.TryGetProperty(fieldName, out var fieldValueElement))
            {
                return defaultValue;
            }
            return fieldValueElement.Deserialize<ResultType>();
        }
    }
}

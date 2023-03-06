namespace Dataverse.RestClient
{
    using System.Runtime.ExceptionServices;
    using System.Text.Json;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class DataverseWebApiException : Exception
    {
        public string Code { get; private set; } = string.Empty;

        public string ExceptionType { get; private set; } = string.Empty;

        public HttpResponseMessage? Response { get; private set; }

        public DataverseWebApiException(JsonElement error)
          : base(GetMessage(error), GetInnerException(error))
        {
            if (error.TryGetProperty("code", out var codeElement))
                this.Code = codeElement.ToString();
            if (error.TryGetProperty("type", out var typeElement))
                this.ExceptionType = typeElement.ToString();
            if (error.TryGetProperty("stacktrace", out var stackTraceElement))
                ExceptionDispatchInfo.SetRemoteStackTrace(this, stackTraceElement.ToString());
        }

        public DataverseWebApiException(JsonElement error, HttpResponseMessage response)
          : this(error)
        {
            this.Response = response;
        }

        public DataverseWebApiException(string message, HttpResponseMessage response)
          : base(message)
        {
            this.Response = response;
        }

        public DataverseWebApiException(string message, Exception innerException)
          : base(message, innerException)
        {
        }

        private static string GetMessage(JsonElement error)
        {
            var message = "An error occurred in the Dataverse Web Api call.";
            if (error.TryGetProperty("message", out var messageElement))
            {
                message = messageElement.ToString();
            }
            return message;
        }

        private static DataverseWebApiException? GetInnerException(JsonElement error)
        {
            if (error.TryGetProperty("innererror", out var innererrorElement))
            {
                return new DataverseWebApiException(innererrorElement);
            }
            return null;
        }

        public static async Task<DataverseWebApiException> Parse(HttpResponseMessage responseMessage)
        {
            var error = await responseMessage.Content.ReadAsStringAsync();
            if (error.IsJSON() && JsonDocument.Parse(error).RootElement.TryGetProperty("error", out var errorElement))
                return new DataverseWebApiException(errorElement, responseMessage);
            return new DataverseWebApiException(error, responseMessage);
        }
    }
}

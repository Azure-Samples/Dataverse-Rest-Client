namespace Dataverse.RestClient
{
    using Dataverse.RestClient.Model;
    using System.IO;
    using System.Text.Json;

    public interface IDataverseClient
    {
        IBatchOperation CreateBatchOperation();

        Task<HttpResponseMessage> DeleteAsync(
          string entitySetName,
          Guid? itemId,
          CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> ListAsStreamAsync(
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
          CancellationToken cancellationToken = default);

        Task<JsonArrayResponse> ListAsync(
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
          CancellationToken cancellationToken = default);

        Task<JsonArrayResponse<TData, TEventArgs>> ListAsync<TData, TEventArgs>(
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
          CancellationToken cancellationToken = default);

        Task<JsonArrayResponse<TData>> ListAsync<TData>(
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
          CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> PatchAsync(
          string entitySetName,
          string jsonData,
          Guid itemId,
          string? subEntitySetName = null,
          bool withRepresentation = false,
          bool withAnnotations = false,
          CancellationToken cancellationToken = default);

        Task<T?> PatchAsync<T>(
            string entitySetName,
            string jsonData,
            Guid itemId,
            string? subEntitySetName = null,
            bool withAnnotations = false,
            Func<JsonElement, T>? convert = null,
            CancellationToken cancellationToken = default);
        
        Task<HttpResponseMessage> PatchAsync(
            string entitySetName,
            Stream data,
            Guid itemId,
            string? subEntitySetName = null,
            bool withRepresentation = false,
            bool withAnnotations = false,
            CancellationToken cancellationToken = default);
        
        Task<HttpResponseMessage> PostAsync(
          string entitySetName,
            string jsonData,
            string? subEntitySetName = null,
            Guid? itemId = null,
            string? select = null,
            string? expand = null,
            bool withRepresentation = false,
            bool withAnnotations = false,
            CancellationToken cancellationToken = default);

        Task<T?> PostAsync<T>(
            string entitySetName,
            string jsonData,
            string? subEntitySetName = null,
            Guid? itemId = null,
            string? select = null,
            string? expand = null,
            bool withAnnotations = false,
            Func<JsonElement, T>? convert = null,
            CancellationToken cancellationToken = default);
        
        Task<EntityReference> PostAsync(
            string entitySetName,
            string jsonData,
            string? subEntitySetName = null,
            Guid? itemId = null,
            CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> SendBatchAsync(
            string batchId,
            IEnumerable<HttpContent> contents,
            CancellationToken cancellationToken = default);

    }
}

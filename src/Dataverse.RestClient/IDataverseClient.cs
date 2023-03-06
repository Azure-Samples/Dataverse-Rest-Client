namespace Dataverse.RestClient
{
    using Dataverse.RestClient.Model;
    using System.IO;
    using System.Text.Json;

    /// <summary>
    /// Dataverse Client to interact with Dataverse Web API. 
    /// </summary>
    public interface IDataverseClient
    {
        /// <summary>
        /// Create a new batch operation.
        /// </summary>
        /// <returns>An instance of batch operation.</returns>
        IBatchOperation CreateBatchOperation();

        /// <summary>
        /// Deletes a record based on the entity set name and the record id.
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="itemId">ID of record.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Response object.</returns>
        Task<HttpResponseMessage> DeleteAsync(
          string entitySetName,
          string? key,
          CancellationToken cancellationToken = default);

        /// <summary>
        /// Get records as stream.
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="requestOptions">Request Options</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Response object.</returns>
        Task<HttpResponseMessage> ListAsStreamAsync(
          string entitySetName,
          RequestOptions? requestOptions = null,
          CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Records as collection of JsonElement
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="requestOptions">Request Options.</param>
        /// <param name="convert">Convert each element of response array into JsonElement</param>
        /// <param name="eventArgs">Arguments to pass to Convert parameter</param>
        /// <param name="previousResponse">Pass Previous Response to get next page.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Collection of JsonElement</returns>
        Task<JsonArrayResponse> ListAsync(
          string entitySetName,
          RequestOptions? requestOptions = null,
          Func<JsonElement, object?, JsonElement>? convert = null,
          object? eventArgs = null,
          JsonArrayResponse? previousResponse = null,
          CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Records as collection of TData
        /// </summary>
        /// <typeparam name="TData">Type of each element in Response array</typeparam>
        /// <typeparam name="TEventArgs">Type of argument to pass to Convert method</typeparam>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="requestOptions">Request Options.</param>
        /// <param name="convert">Convert each element of response array into TData</param>
        /// <param name="eventArgs">Arguments to pass to Convert parameter</param>
        /// <param name="previousResponse">Pass Previous Response to get next page.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Collection of TData</returns>
        Task<JsonArrayResponse<TData, TEventArgs>> ListAsync<TData, TEventArgs>(
          string entitySetName,
          RequestOptions? requestOptions = null,
          Func<JsonElement, TEventArgs?, TData>? convert = null,
          TEventArgs? eventArgs = default,
          JsonArrayResponse<TData, TEventArgs>? previousResponse = null,
          CancellationToken cancellationToken = default);

        /// <summary>
        /// Get Records as collection of TData
        /// </summary>
        /// <typeparam name="TData">Type of each element in Response array</typeparam>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="requestOptions">Request Options.</param>
        /// <param name="convert">Convert each element of response array into TData</param>
        /// <param name="eventArgs">Arguments to pass to Convert parameter</param>
        /// <param name="previousResponse">Pass Previous Response to get next page.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Collection of TData</returns>
        Task<JsonArrayResponse<TData>> ListAsync<TData>(
          string entitySetName,
          RequestOptions? requestOptions = null,
          Func<JsonElement, object?, TData>? convert = null,
          object? eventArgs = null,
          JsonArrayResponse<TData>? previousResponse = null,
          CancellationToken cancellationToken = default);

        /// <summary>
        /// Perform PATCH (update) operation against entity using Dataverse WebAPI.
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="jsonData">Payload content. Data that needs to be updated.</param>
        /// <param name="key">Either GUID of the record or alternate key as comma separated key=value pair.</param>
        /// <param name="subEntitySetName">Any property name on the entity.</param>
        /// <param name="withRepresentation">True returns the data with the result, False returns no body.</param>
        /// <param name="withAnnotations">True will return formatted values in response.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Response Object.</returns>
        Task<HttpResponseMessage> PatchAsync(
          string entitySetName,
          string jsonData,
          string key,
          string? subEntitySetName = null,
          bool withRepresentation = false,
          bool withAnnotations = false,
          CancellationToken cancellationToken = default);

        /// <summary>
        /// Perform PATCH (update) operation against entity using Dataverse WebAPI.
        /// </summary>
        /// <typeparam name="T">Type representing entity.</typeparam>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="jsonData">Payload content. Data that needs to be updated.</param>
        /// <param name="key">Either GUID of the record or alternate key as comma separated key=value pair.</param>
        /// <param name="subEntitySetName">Any property name on the entity.</param>
        /// <param name="withAnnotations">True will return formatted values in response.</param>
        /// <param name="convert">Convert each element of response array into T.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Instance of T</returns>
        Task<T?> PatchAsync<T>(
            string entitySetName,
            string jsonData,
            string key,
            string? subEntitySetName = null,
            bool withAnnotations = false,
            Func<JsonElement, T>? convert = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Upload a Stream (file) to an record.
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="data">File that needs to be uploaded.</param>
        /// <param name="key">Either GUID of the record or alternate key as comma separated key=value pair.</param>
        /// <param name="subEntitySetName">Any property name on the entity.</param>
        /// <param name="withRepresentation">True returns the data with the result, False returns no body.</param>
        /// <param name="withAnnotations">True will return formatted values in response.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Response Object.</returns>
        Task<HttpResponseMessage> PatchAsync(
            string entitySetName,
            Stream data,
            string key,
            string? subEntitySetName = null,
            bool withRepresentation = false,
            bool withAnnotations = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create a new record.    
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="jsonData">JSON payload to create a new record in Dataverse.</param>
        /// <param name="subEntitySetName">Any property name that represents relationship.</param>
        /// <param name="key">ID or Alternate key. Used when adding a record in collection.</param>
        /// <param name="select">Comma separated field names to return in the result.</param>
        /// <param name="expand">Expand a navigation property.</param>
        /// <param name="withRepresentation">True returns the data with the result, False returns no body.</param>
        /// <param name="withAnnotations">True will return formatted values in response.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Response object.</returns>
        Task<HttpResponseMessage> PostAsync(
            string entitySetName,
            string jsonData,
            string? subEntitySetName = null,
            string? key = null,
            string? select = null,
            string? expand = null,
            bool withRepresentation = false,
            bool withAnnotations = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create a new record.  
        /// </summary>
        /// <typeparam name="T">Type of result to convert to.</typeparam>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="jsonData">JSON payload to create a new record in Dataverse.</param>
        /// <param name="subEntitySetName">Any property name that represents relationship.</param>
        /// <param name="key">ID or Alternate key. Used when adding a record in collection.</param>
        /// <param name="select">Comma separated field names to return in the result.</param>
        /// <param name="expand">Expand a navigation property.</param>
        /// <param name="withAnnotations">True will return formatted values in response.</param>
        /// <param name="convert">Convert the result into T.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Instance of type T.</returns>
        Task<T?> PostAsync<T>(
            string entitySetName,
            string jsonData,
            string? subEntitySetName = null,
            string? key = null,
            string? select = null,
            string? expand = null,
            bool withAnnotations = false,
            Func<JsonElement, T>? convert = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create a new record.
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="jsonData">JSON payload to create a new record in Dataverse.</param>
        /// <param name="subEntitySetName">Any property name that represents relationship.</param>
        /// <param name="key">ID or Alternate key. Used when adding a record in collection.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Instance of <see cref="EntityReference"/>.</returns>
        Task<EntityReference> PostAsync(
            string entitySetName,
            string jsonData,
            string? subEntitySetName = null,
            string? key = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a batch request comprising of <paramref name="contents"/>.
        /// </summary>
        /// <param name="batchId">Creates batch request with this ID.</param>
        /// <param name="contents">Contents that needs to be sent in the batch request.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        /// <returns>Response Object.</returns>
        Task<HttpResponseMessage> SendBatchAsync(
            string batchId,
            IEnumerable<HttpContent> contents,
            CancellationToken cancellationToken = default);

    }
}

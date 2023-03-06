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

    /// <summary>
    /// Manages requests that are sent in a single batch.
    /// </summary>
    public interface IBatchOperation
    {
        /// <summary>
        /// Send the batch request and parse the response.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<BatchOperationResult>> ProcessAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a <see cref="HttpContent"/> to the batch.
        /// </summary>
        /// <param name="content">HttpContent</param>
        /// <returns>Request ID. Used to correlate with response.</returns>
        int AddContent(HttpContent content);

        /// <summary>
        /// Add a <see cref="IChangeSet"/> to the batch.
        /// </summary>
        /// <param name="changeSet">ChangeSet</param>
        /// <returns>Request ID. Used to correlate with response.</returns>
        int AddChangeSet(IChangeSet changeSet);

        /// <summary>
        /// Add a Create request to the batch.
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="jsonData">JSON to create new record.</param>
        /// <returns>Request ID. Used to correlate with response.</returns>
        int AddCreate(string entitySetName, string jsonData)
        {
            var message = CreateHttpMessageRequest(DataverseClient.BuildRequestUrl(entitySetName), "POST", jsonData);
            return AddContent(message.ToBatchHttpMessageContent());
        }

        /// <summary>
        /// Add a Delete request to the batch.
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="key">ID or alternate key of record to delete.</param>
        /// <returns>Request ID. Used to correlate with response.</returns>
        int AddDelete(string entitySetName, string key)
        {
            var message = CreateHttpMessageRequest(DataverseClient.BuildRequestUrl(entitySetName, key: key), "DELETE");
            return AddContent(message.ToBatchHttpMessageContent());
        }

        /// <summary>
        /// Add a request to retrieve records based on filter criterias.
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="requestOptions">Options for OData query.</param>
        /// <returns>Request ID. Used to correlate with response.</returns>
        int AddRead(
            string entitySetName,
            RequestOptions requestOptions)
        {
            var message = CreateHttpMessageRequest(DataverseClient.BuildRequestUrl(
                entitySetName,
                requestOptions?.SubEntitySetName,
                requestOptions?.GetKey(),
                requestOptions?.FetchXml,
                requestOptions?.Select,
                requestOptions?.Filter,
                requestOptions?.OrderBy,
                requestOptions?.Expand,
                requestOptions?.Top,
                requestOptions?.ExpandSelect), "GET");
            return AddContent(message.ToBatchHttpMessageContent());
        }

        /// <summary>
        /// Add a request to update a record.
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="key">ID or alternate key of the record to update.</param>
        /// <param name="jsonData">JSON to update.</param>
        /// <returns>Request ID. Used to correlate with response.</returns>
        int AddUpdate(string entitySetName, string key, string jsonData)
        {
            var message = CreateHttpMessageRequest(DataverseClient.BuildRequestUrl(entitySetName, null, key), "PATCH", jsonData);
            return AddContent(message.ToBatchHttpMessageContent());
        }

        /// <summary>
        /// Create a HttpMessageRequest.
        /// </summary>
        /// <param name="url">Relative URL of the entity.</param>
        /// <param name="method">Request Method.</param>
        /// <param name="body">JOSN Body.</param>
        /// <returns>HttpRequestMessage</returns>
        HttpRequestMessage CreateHttpMessageRequest(string url, string method = "POST", string body = "{}");

        /// <summary>
        /// Create a new ChangeSet
        /// </summary>
        /// <returns>ChangeSet</returns>
        IChangeSet CreateChangeSet();
    }
}

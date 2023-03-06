namespace Dataverse.RestClient
{
    using System;
    using System.Net.Http;

    /// <summary>
    /// Represents a ChangeSet in a batch request. If any request in ChangeSet fails, all fails.
    /// </summary>
    public interface IChangeSet
    {
        /// <summary>
        /// Add a Create request to changeset.
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="jsonData">JSON serialized data.</param>
        /// <returns>Request ID. Used to correlate with response.</returns>
        int AddCreate(string entitySetName, string jsonData);

        /// <summary>
        /// Add a Delete request to changeset.
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="key">ID Or Alternate Key.</param>
        /// <returns>Request ID. Used to correlate with response.</returns>
        int AddDelete(string entitySetName, string key);

        /// <summary>
        /// Add HTTPContent to the changeset.
        /// </summary>
        /// <param name="request">HttpContent</param>
        /// <returns>Request ID. Used to correlate with response.</returns>
        int AddHttpRequestMessage(HttpContent request);

        /// <summary>
        /// Add Update request to changeset.
        /// </summary>
        /// <param name="entitySetName">EntitySetName of the entity.</param>
        /// <param name="key">ID Or Alternate Key.</param>
        /// <param name="jsonData">JSON serialized data.</param>
        /// <returns>Request ID. Used to correlate with response.</returns>
        int AddUpdate(string entitySetName, string key, string jsonData);

        /// <summary>
        /// Convert ChangeSet to MultipartContent.
        /// </summary>
        /// <returns>MultipartContent</returns>
        MultipartContent ToMultipartContent();

        /// <summary>
        /// Parses the response into collection of <see cref="ChangeSetResult"/>
        /// </summary>
        /// <param name="content">Response to the BatchRequest.</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Collection of ChangeSetResult.</returns>
        Task<List<ChangeSetResult>> ProcessAsync(HttpContent content, CancellationToken cancellationToken);
    }
}
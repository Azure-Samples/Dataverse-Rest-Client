namespace Dataverse.RestClient
{
    using System.Text.Json;

    public static class DataverseExtension
    {

        public static async Task<EntityMetadata?> GetEntityMetadataAsync(
          this DataverseClient dataverseClient,
          string logicalName,
          bool expandAttributes = true)
        {
            var metatadatas = await dataverseClient.ListAsync("EntityDefinitions",
                                                              null,
                                                              new Guid?(),
                                                              null,
                                                              "MetadataId, LogicalName, DisplayName, EntitySetName, PrimaryIdAttribute, PrimaryNameAttribute",
                                                              null,
                                                              "LogicalName eq '" + logicalName + "'",
                                                              new int?(),
                                                              null,
                                                              expandAttributes ? "Attributes" : null,
                                                              null,
                                                              false,
                                                              (metadataJson, eventArgs) => new EntityMetadata(metadataJson),
                                                              null,
                                                              null);
            return metatadatas.FirstOrDefault();
        }

        public static async Task<IEnumerable<EntityMetadata>> GetAllEntityMetadataAsync(
          this DataverseClient dataverseClient,
          bool expandAttributes = true)
        {
            JsonArrayResponse? metadataRecordsResponse = null;
            var metadataRecords = new List<EntityMetadata>();
            do
            {
                metadataRecordsResponse = await dataverseClient.ListAsync("EntityDefinitions",
                                                                              null,
                                                                              new Guid?(),
                                                                              null,
                                                                              "MetadataId, LogicalName, DisplayName, EntitySetName, PrimaryIdAttribute, PrimaryNameAttribute",
                                                                              null,
                                                                              null,
                                                                              new int?(),
                                                                              null,
                                                                              null,
                                                                              null,
                                                                              false,
                                                                              null,
                                                                              null,
                                                                              metadataRecordsResponse);
                foreach (JsonElement metadataJson in (JsonArrayResponse<JsonElement, object>)metadataRecordsResponse)
                    metadataRecords.Add(new EntityMetadata(metadataJson));
                metadataRecordsResponse.Clear();
            }
            while (!string.IsNullOrEmpty(metadataRecordsResponse.NextLink));
            return metadataRecords;
        }
    }
}

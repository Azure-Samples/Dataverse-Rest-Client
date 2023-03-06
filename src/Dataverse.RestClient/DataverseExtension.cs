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
                new RequestOptions(
                    select: "MetadataId, LogicalName, DisplayName, EntitySetName, PrimaryIdAttribute, PrimaryNameAttribute",
                    filter: $"LogicalName eq '{logicalName}'",
                    expand: expandAttributes ? "Attributes" : null,
                    withAnnotations: false
                ),
                convert: (metadataJson, eventArgs) => new EntityMetadata(metadataJson));
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
                     new RequestOptions(
                    select: "MetadataId, LogicalName, DisplayName, EntitySetName, PrimaryIdAttribute, PrimaryNameAttribute",
                    expand: expandAttributes ? "Attributes" : null,
                    withAnnotations: false),
                    previousResponse: metadataRecordsResponse);
                foreach (JsonElement metadataJson in (JsonArrayResponse<JsonElement, object>)metadataRecordsResponse)
                    metadataRecords.Add(new EntityMetadata(metadataJson));
                metadataRecordsResponse.Clear();
            }
            while (!string.IsNullOrEmpty(metadataRecordsResponse.NextLink));
            return metadataRecords;
        }
    }
}

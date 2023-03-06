using System.Text.Json.Serialization;

namespace Samples.WebAPI.Models.Response
{
    public class Account
    {
        [JsonPropertyName("accountid")]
        public Guid AccountId { get; set; }

        public string Name { get; set; }

        [JsonPropertyName("_primarycontactid_value")]
        public Guid? PrimaryContactId { get; set; }

        [JsonPropertyName("_primarycontactid_value@OData.Community.Display.V1.FormattedValue")]
        public string PrimaryContactName { get; set; }
    }
}

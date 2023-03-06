using System.Text.Json.Serialization;

namespace Samples.WebAPI.Models.Request
{
    public class Account
    {
        private string primaryContactId;

        public string Name { get; set; }

        [JsonPropertyName("primarycontactid@odata.bind")]
        public string? PrimaryContact
        {
            get
            {
                return string.IsNullOrEmpty(this.primaryContactId) ? null : $"/contacts({this.primaryContactId})";
            }

            set
            {
                this.primaryContactId = value;
            }
        }
    }
}

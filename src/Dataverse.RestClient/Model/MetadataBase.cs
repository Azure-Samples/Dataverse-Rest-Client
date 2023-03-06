namespace Dataverse.RestClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public abstract class MetadataBase
    {
        private Guid metadataId = Guid.Empty;
        private string logicalName = string.Empty;
        private DisplayName? displayName = null;

        protected JsonElement MetadataJson { get; set; }

        public virtual Guid Id
        {
            get
            {
                if (this.metadataId.Equals(Guid.Empty) && this.MetadataJson.TryGetProperty("MetadataId", out var metadataElement))
                {
                    this.metadataId = metadataElement.GetGuid();
                }
                return this.metadataId;
            }
        }

        public virtual string LogicalName
        {
            get
            {
                if (string.IsNullOrEmpty(this.logicalName) && this.MetadataJson.TryGetProperty(nameof(LogicalName), out var logicalNameElement))
                    this.logicalName = logicalNameElement.ToString();
                return this.logicalName;
            }
        }

        public virtual DisplayName? DisplayName
        {
            get
            {
                if (this.displayName == null && this.MetadataJson.TryGetProperty(nameof(DisplayName), out var displayNameElement))
                {
                    this.displayName = new DisplayName(displayNameElement);
                }
                return displayName;
            }
        }

        public string EnglishDisplayName => this.DisplayName?.EnglishDisplayName ?? this.LogicalName;

        public string this[string fieldName] => this.MetadataJson.GetProperty(fieldName).ToString();

        protected MetadataBase(JsonElement metadataJson) => this.MetadataJson = metadataJson;

        public virtual IEnumerable<string> GetFieldNames() => this.MetadataJson.EnumerateObject().Select(e => e.Name);

        public virtual ResultType? GetFieldValue<ResultType>(string fieldName, ResultType? defaultValue = default) => this.MetadataJson.DeserializeOrDefault(fieldName, defaultValue);
    }
}

namespace Dataverse.RestClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;

    public class EntityMetadata : MetadataBase
    {
        private string entitySetName = string.Empty;
        private string primaryIdAttribute = string.Empty;
        private string primaryNameAttribute = string.Empty;
        [NonSerialized]
        private readonly IDictionary<string, AttributeMetadata> attributes;

        public string EntitySetName
        {
            get
            {
                if (string.IsNullOrEmpty(this.entitySetName) && this.MetadataJson.TryGetProperty(nameof(EntitySetName), out var entitySetNameElement))
                {
                    this.entitySetName = entitySetNameElement.ToString();
                }
                return this.entitySetName;
            }
        }

        public string PrimaryIdAttribute
        {
            get
            {
                if (string.IsNullOrEmpty(this.primaryIdAttribute) && this.MetadataJson.TryGetProperty(nameof(PrimaryIdAttribute), out var primaryIdAttributeElement))
                {
                    this.primaryIdAttribute = primaryIdAttributeElement.ToString();
                }
                return this.primaryIdAttribute;
            }
        }

        public string PrimaryNameAttribute
        {
            get
            {
                if (string.IsNullOrEmpty(this.primaryNameAttribute) && this.MetadataJson.TryGetProperty(nameof(PrimaryNameAttribute), out var primaryNameAttributeElement))
                {
                    this.primaryNameAttribute = primaryNameAttributeElement.ToString();
                }
                return this.primaryNameAttribute;
            }
        }

        public bool HasAttributes => this.attributes != null && this.attributes.Any();

        public IDictionary<string, AttributeMetadata> Attributes => this.attributes;

        public EntityMetadata(JsonElement metadataJson)
          : base(metadataJson)
        {
            this.attributes = new Dictionary<string, AttributeMetadata>();
            var attributeElements = new JsonElement();
            metadataJson.TryGetProperty("Attributes", out attributeElements);
            foreach (var item in attributeElements.EnumerateArray())
            {
                attributes[item.GetProperty("LogicalName").ToString()] = new AttributeMetadata(item);
            }
        }
    }
}

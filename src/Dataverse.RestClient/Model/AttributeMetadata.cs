namespace Dataverse.RestClient
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.Json;

    public class AttributeMetadata : MetadataBase
    {
        private string attributeType = string.Empty;
        private string format = string.Empty;

        public virtual string AttributeType
        {
            get
            {
                if (string.IsNullOrEmpty(this.attributeType) && this.MetadataJson.TryGetProperty(nameof(AttributeType), out var attributeTypeElement))
                {
                    this.attributeType = attributeTypeElement.ToString();
                }
                return this.attributeType;
            }
        }

        public virtual string Format
        {
            get
            {
                if (string.IsNullOrEmpty(this.format) && this.MetadataJson.TryGetProperty(nameof(Format), out var formatElement))
                {
                    this.format = formatElement.ToString();
                }
                return this.format;
            }
        }

        public AttributeMetadata(JsonElement metadataJson)
        : base(metadataJson)
        {
        }
        public virtual IEnumerable<string?> GetAttributeOptions()
        {
            return this.GetFieldValue("OptionSet", new JsonElement()).EnumerateArray()
                .Select(option => option.GetProperty("DisplayName"))
                .Select(displayNameMetadataJson => new DisplayName(displayNameMetadataJson))
                .Select(displayName => displayName.EnglishDisplayName);
        }
    }
}

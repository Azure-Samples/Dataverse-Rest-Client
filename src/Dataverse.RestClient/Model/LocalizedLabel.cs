namespace Dataverse.RestClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    [Serializable]
    public class LocalizedLabel
    {
        public string Label { get; protected set; } = string.Empty;

        public int? LanguageCode { get; protected set; }

        public LocalizedLabel(JsonElement localizedLabel)
        {
            if (localizedLabel.TryGetProperty(nameof(Label), out var labelElement))
            {
                this.Label = labelElement.ToString();
            }
            if (localizedLabel.TryGetProperty(nameof(LanguageCode), out var languageCode))
            {
                this.LanguageCode = languageCode.GetInt32();
            }
        }

        public LocalizedLabel(string label, int? languageCode)
        {
            this.Label = label;
            this.LanguageCode = languageCode;
        }

        public LocalizedLabel(XElement localizedLabel)
        {
            this.Label = localizedLabel.Attribute((XName)"description")?.Value ?? string.Empty;
            this.LanguageCode = (int?)localizedLabel.Attribute((XName)"languagecode");
        }
    }
}

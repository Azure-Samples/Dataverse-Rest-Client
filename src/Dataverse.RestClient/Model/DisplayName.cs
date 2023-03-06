namespace Dataverse.RestClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Xml.Linq;

    [Serializable]
    public class DisplayName
    {
        public const int EnglishLanguageCode = 1033;

        public IEnumerable<LocalizedLabel> LocalizedLabels { get; protected set; }

        public string? EnglishDisplayName => this.LocalizedLabels.Where(localizedLabel =>
        {
            int? languageCode = localizedLabel.LanguageCode;
            int num = 1033;
            return languageCode.GetValueOrDefault() == num && languageCode.HasValue;
        }).Select(localizedLabel => localizedLabel.Label).FirstOrDefault();

        public DisplayName(JsonElement displayName, string localizedLabelElementName = "LocalizedLabels")
        {
            var localizedLabels = new List<JsonElement>();
            if (displayName.TryGetProperty(localizedLabelElementName, out var localizedLabelElement))
            {
                localizedLabels = localizedLabelElement.EnumerateArray().ToList();
            }
            this.LocalizedLabels = localizedLabels.Select(localizedLabel => new LocalizedLabel(localizedLabel)).ToList();
        }

        public DisplayName(IEnumerable<XElement> displayName) => this.LocalizedLabels = displayName.Select(label => new LocalizedLabel(label));
    }
}

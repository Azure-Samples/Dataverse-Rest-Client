using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataverse.RestClient
{
    /// <summary>
    /// Represents a set of options for a GET request.
    /// </summary>
    public class RequestOptions
    {
        /// <summary>
        /// A property in the entity. The URL is build as `/EntitySetName(ID)/SubEntityStName`.
        /// </summary>
        public string? SubEntitySetName { get; set; }

        /// <summary>
        /// ID of their record. Set either ID or Alternate Key.
        /// </summary>
        public Guid? ItemId { get; set; }

        /// <summary>
        /// Alternate Key of the record. Set either ID or Alternate Key.
        /// </summary>
        public string? AlternateKey { get; set; }
        
        /// <summary>
        /// Fetch data using FetchXML.
        /// </summary>
        public string? FetchXml { get; set; }

        /// <summary>
        /// Comma separated column names.
        /// </summary>
        public string? Select { get; set; }

        /// <summary>
        /// Filter operation to perform.
        /// </summary>
        public string? Filter { get; set; }

        /// <summary>
        /// Specify number of results to return.
        /// </summary>
        public int? Top { get; set; }

        /// <summary>
        /// Order by the result.
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// Expand a navigation property in an entity.
        /// </summary>
        public string? Expand { get; set; }

        /// <summary>
        /// Select the properties of the expanded property.
        /// </summary>
        public string? ExpandSelect { get; set; }

        /// <summary>
        /// True will return formatted values in response.
        /// </summary>
        public bool WithAnnotations { get; set; }

        /// <summary>
        /// Create empty options instance.
        /// </summary>
        public RequestOptions()
        { }

        public RequestOptions(
            Guid itemId,
            string? subEntitySetName = null,
            string? select = null,
            string? filter = null,
            int? top = null,
            string? expand = null,
            string? expandSelect = null,
            bool withAnnotations = true)
        {
            SubEntitySetName = subEntitySetName;
            ItemId = itemId;
            Select = select;
            Filter = filter;
            Top = top;
            Expand = expand;
            ExpandSelect = expandSelect;
            WithAnnotations = withAnnotations;
        }

        public RequestOptions(
            string alternateKey,
            string? subEntitySetName = null,
            string? select = null,
            string? filter = null,
            int? top = null,
            string? expand = null,
            string? expandSelect = null,
            bool withAnnotations = true)
        {
            SubEntitySetName = subEntitySetName;
            AlternateKey = alternateKey;
            Select = select;
            Filter = filter;
            Top = top;
            Expand = expand;
            ExpandSelect = expandSelect;
            WithAnnotations = withAnnotations;
        }

        public RequestOptions(
            string? subEntitySetName = null,
            string? select = null,
            string? filter = null,
            int? top = null,
            string? expand = null,
            string? expandSelect = null,
            bool withAnnotations = true)
        {
            SubEntitySetName = subEntitySetName;
            Select = select;
            Filter = filter;
            Top = top;
            Expand = expand;
            ExpandSelect = expandSelect;
            WithAnnotations = withAnnotations;
        }

        public RequestOptions(string fetchXml)
        {
            FetchXml = fetchXml;
        }

        public string? GetKey()
        {
            if (ItemId.HasValue)
                return ItemId.Value.ToCleanString();
            return AlternateKey;
        }
    }
}

namespace Dataverse.RestClient.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    public class Entity
    {
        private string entityLogicalName = string.Empty;
        private string primaryIdAttribute = string.Empty;
        private Guid entityId = Guid.Empty;

        public Guid Id
        {
            get
            {
                if (this.entityId.Equals(Guid.Empty)
                    && !string.IsNullOrEmpty(this.primaryIdAttribute)
                    && this.EntityJson.TryGetProperty(this.primaryIdAttribute, out var idElement))
                {
                    this.entityId = idElement.GetGuid();
                }

                return this.entityId;
            }
        }

        public string LogicalName => this.entityLogicalName;

        protected JsonElement EntityJson { get; private set; }

        public Entity(JsonElement entityJson, string entityLogicalName, string primaryIdAttribute)
        {
            this.EntityJson = entityJson;
            this.entityLogicalName = entityLogicalName;
            this.primaryIdAttribute = primaryIdAttribute;
        }

        public Entity(JsonElement entityJson, EntityMetadata entityMetadata)
          : this(entityJson, entityMetadata.LogicalName, entityMetadata.PrimaryIdAttribute)
        {
        }

        public IEnumerable<string> GetFieldNames() => this.EntityJson.EnumerateObject().Select(e => e.Name);

        public ResultType? GetFieldValue<ResultType>(string fieldName, ResultType? defaultValue = default) => this.EntityJson.DeserializeOrDefault(fieldName, defaultValue);

    }
}

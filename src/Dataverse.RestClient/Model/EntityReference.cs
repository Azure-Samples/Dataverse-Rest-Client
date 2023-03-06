namespace Dataverse.RestClient.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EntityReference
    {
        public Guid? Id { get; internal set; }

        public string EntitySetName { get; internal set; }

        public Dictionary<string, string>? AlternateKeyAttributes { get; set; }

        public EntityReference(string entitySetName, Guid id)
        {
            this.EntitySetName = entitySetName;
            this.Id = id;
        }

        public EntityReference(string entitySetName, Dictionary<string, string> alternateKeyAttributes)
        {
            this.EntitySetName = entitySetName;
            this.AlternateKeyAttributes = alternateKeyAttributes;
        }

        public EntityReference(string uri)
        {
            int firstParen = uri.LastIndexOf('(');
            int lastParen = uri.LastIndexOf(')');
            int lastBackSlash = uri.LastIndexOf('/') + 1;
            if (lastBackSlash >= 0 && firstParen > lastBackSlash && lastParen > firstParen)
            {
                EntitySetName = uri[lastBackSlash..firstParen];

                firstParen++;

                if (Guid.TryParse(uri[firstParen..lastParen], out Guid id))
                {
                    Id = id;
                }
                else
                {
                    //It may be an alternate key.
                    try
                    {
                        AlternateKeyAttributes = uri[firstParen++..lastParen]
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(part => part.Split('='))
                            .ToDictionary(split => split[0], split => split[1]);
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("Invalid Uri");
                    }
                }
            }
            else
            {
                throw new ArgumentException("Invalid Uri");
            }
        }
    }
}

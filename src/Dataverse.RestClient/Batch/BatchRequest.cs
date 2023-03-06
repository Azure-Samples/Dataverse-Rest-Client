namespace Dataverse.RestClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class BatchRequest
    {
        public int Index { get; }
        public HttpContent HttpContent { get; }
        public IChangeSet? ChangeSet { get; }
        public bool IsChangeSetRequest { get; }

        public BatchRequest(int index, HttpContent httpContent, IChangeSet? changeSet = null)
        {
            this.Index = index;
            this.HttpContent = httpContent;
            this.ChangeSet = changeSet;
            this.IsChangeSetRequest = changeSet != null;
        }
    }
}

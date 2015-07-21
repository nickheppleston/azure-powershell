using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace Microsoft.WindowsAzure.Commands.Utilities.DocumentDb
{
    public class ExtendedDocumentDbCollection
    {
        public ExtendedDocumentDbCollection()
        {
        }

        public ExtendedDocumentDbCollection(DocumentCollection documentDbCollection)
        {
            ETag = documentDbCollection.ETag;
            Id = documentDbCollection.Id;
            ResourceId = documentDbCollection.ResourceId;
            SelfLink = documentDbCollection.SelfLink;
            Timestamp = documentDbCollection.Timestamp;
            IndexingPolicy = documentDbCollection.IndexingPolicy;
            ConflictsLink = documentDbCollection.ConflictsLink;
            DocumentsLink = documentDbCollection.DocumentsLink;
            StoredProceduresLink = documentDbCollection.StoredProceduresLink;
            UserDefinedFunctionsLink = documentDbCollection.UserDefinedFunctionsLink;
        }

        public string ETag { get; set; }
        public string Id { get; set; }
        public string ResourceId { get; set; }
        public string SelfLink { get; set; }
        public DateTime Timestamp { get; set; }
        public IndexingPolicy IndexingPolicy { get; set; }
        public string ConflictsLink { get; set; }
        public string DocumentsLink { get; set; }
        public string StoredProceduresLink { get; set; }
        public string UserDefinedFunctionsLink { get; set; }
    }
}

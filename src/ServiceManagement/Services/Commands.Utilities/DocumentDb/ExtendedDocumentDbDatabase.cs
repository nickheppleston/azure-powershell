using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace Microsoft.WindowsAzure.Commands.Utilities.DocumentDb
{
    public class ExtendedDocumentDbDatabase
    {
        public ExtendedDocumentDbDatabase()
        {
        }

        public ExtendedDocumentDbDatabase(Database documentDbDatabase)
        {
            ETag = documentDbDatabase.ETag;
            Id = documentDbDatabase.Id;
            ResourceId = documentDbDatabase.ResourceId;
            SelfLink = documentDbDatabase.SelfLink;
            Timestamp = documentDbDatabase.Timestamp;
            CollectionsLink = documentDbDatabase.CollectionsLink;
            UsersLink = documentDbDatabase.UsersLink;
        }

        public string ETag { get; set; }
        public string Id { get; set; }
        public string ResourceId { get; set; }
        public string SelfLink { get; set; }
        public DateTime Timestamp { get; set; }
        public string CollectionsLink { get; set; }
        public string UsersLink { get; set; }
    }
}

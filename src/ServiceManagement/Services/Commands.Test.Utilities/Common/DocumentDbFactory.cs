using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Commands.Utilities.DocumentDb;

namespace Microsoft.WindowsAzure.Commands.Test.Utilities.Common
{
    public static class DocumentDbFactory
    {
        public static ExtendedDocumentDbCollection CreateExtendedDocumentDbCollection(string id)
        {
            return (new ExtendedDocumentDbCollection()
                {
                    ETag = "00000700-0000-0000-0000-65ab02780000",
                    Id = id,
                    ResourceId = "J5jkAMPpIwE=",
                    SelfLink = "dbs/J6kkAA==/colls/J5jkAMPpIwE=/",
                    Timestamp = DateTime.Now,
                    IndexingPolicy = null, // We are not handling Indexing Policy in this release.
                    ConflictsLink = "dbs/J6kkAA==/colls/J5jkAMPpIwE=/conflicts/",
                    DocumentsLink = "dbs/J6kkAA==/colls/J5jkAMPpIwE=/docs/",
                    StoredProceduresLink = "dbs/J6kkAA==/colls/J5jkAMPpIwE=/sprocs/",
                    UserDefinedFunctionsLink = "dbs/J6kkAA==/colls/J5jkAMPpIwE=/udfs/",
                });
        }
    }
}

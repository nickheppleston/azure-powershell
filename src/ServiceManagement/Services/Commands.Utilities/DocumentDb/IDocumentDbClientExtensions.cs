using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace Microsoft.WindowsAzure.Commands.Utilities.DocumentDb
{
    public interface IDocumentDbClientExtensions
    {
        List<ExtendedDocumentDbDatabase> GetDatabases();
        ExtendedDocumentDbDatabase GetDatabase(string id);
        Task<ExtendedDocumentDbDatabase> CreateDatabaseAsync(string id);
        Task DeleteDatabaseAsync(string selfLink);
        
        List<ExtendedDocumentDbCollection> GetDocumentCollections(string collectionsLink);
        ExtendedDocumentDbCollection GetDocumentCollection(string collectionsLink, string id);
        Task<ExtendedDocumentDbCollection> CreateDocumentCollectionsAsync(string databaseLink, string id);
        Task DeleteDocumentCollectionsAsync(string selfLink);
    }
}

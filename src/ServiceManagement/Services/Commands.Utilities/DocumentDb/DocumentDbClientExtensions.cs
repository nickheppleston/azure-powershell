using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Common.Authentication;
using Microsoft.Azure.Common.Authentication.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Microsoft.WindowsAzure.Commands.Utilities.DocumentDb
{
    public class DocumentDbClientExtensions : IDocumentDbClientExtensions
    {
        private readonly DocumentClient _documentDbClient;

        public DocumentDbClientExtensions(string serviceEndpoint, string authKey)
        {
            _documentDbClient = _documentDbClient ?? new DocumentClient(GetServiceEndpointUri(serviceEndpoint), authKey);
        }

        #region DocumentDb Databases

        public List<ExtendedDocumentDbDatabase> GetDatabases()
        {
            return (_documentDbClient.CreateDatabaseQuery().AsEnumerable<Database>().Select(db => new ExtendedDocumentDbDatabase(db)).ToList<ExtendedDocumentDbDatabase>());
        }

        public ExtendedDocumentDbDatabase GetDatabase(string id)
        {
            return (_documentDbClient.CreateDatabaseQuery().Where(d => d.Id.Equals(id)).AsEnumerable<Database>().Select(db => new ExtendedDocumentDbDatabase(db)).FirstOrDefault());
        }

        public async Task<ExtendedDocumentDbDatabase> CreateDatabaseAsync(string id)
        {
            var documentDbDatabase = await _documentDbClient.CreateDatabaseAsync(new Database() { Id = id });

            return (new ExtendedDocumentDbDatabase(documentDbDatabase));
        }

        public async Task DeleteDatabaseAsync(string selfLink)
        {
            await _documentDbClient.DeleteDatabaseAsync(selfLink);
        }

        #endregion

        #region DocumentDb Collections

        public List<ExtendedDocumentDbCollection> GetDocumentCollections(string collectionsLink)
        {
            return (_documentDbClient.CreateDocumentCollectionQuery(collectionsLink).AsEnumerable<DocumentCollection>().Select(col => new ExtendedDocumentDbCollection(col)).ToList<ExtendedDocumentDbCollection>());
        }

        public ExtendedDocumentDbCollection GetDocumentCollection(string collectionsLink, string id)
        {
            return (_documentDbClient.CreateDocumentCollectionQuery(collectionsLink).Where(d => d.Id.Equals(id)).AsEnumerable<DocumentCollection>().Select(col => new ExtendedDocumentDbCollection(col)).FirstOrDefault());
        }

        public async Task<ExtendedDocumentDbCollection> CreateDocumentCollectionsAsync(string databaseLink, string id)
        {
            var documentDbDocumentCollection = await _documentDbClient.CreateDocumentCollectionAsync(databaseLink, new DocumentCollection() { Id = id });

            return (new ExtendedDocumentDbCollection(documentDbDocumentCollection));
        }

        public async Task DeleteDocumentCollectionsAsync(string selfLink)
        {
            await _documentDbClient.DeleteDocumentCollectionAsync(selfLink);
        }

        #endregion

        #region Private Methods

        private static Uri GetServiceEndpointUri(string serviceEndpoint)
        {
            try
            {
                return (new Uri(serviceEndpoint));
            }
            catch (UriFormatException uriFormatException)
            {
                throw new Exception(string.Format("Failed to parse the supplied Service Endpoint '{0}' into a Uri - Error: {1}", serviceEndpoint, uriFormatException.Message));
            }
        }

        #endregion 

    }
}

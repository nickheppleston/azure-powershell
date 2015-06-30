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
        List<Database> GetDatabases();
        Database GetDatabase(string id);
        Task<Database> CreateDatabaseAsync(string id);
        Task DeleteDatabaseAsync(string databaseLink);
    }
}

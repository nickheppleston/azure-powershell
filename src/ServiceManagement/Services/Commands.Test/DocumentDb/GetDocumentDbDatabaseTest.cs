// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.WindowsAzure.Commands.ScenarioTest;
using Microsoft.WindowsAzure.Commands.Common;
using Microsoft.WindowsAzure.Commands.Common.Test.Mocks;
using Microsoft.WindowsAzure.Commands.DocumentDb;
using Microsoft.WindowsAzure.Commands.Test.Utilities.Common;
using Microsoft.WindowsAzure.Commands.Utilities.Properties;
using Microsoft.WindowsAzure.Commands.Utilities.DocumentDb;
using Xunit;
using Moq;
using Microsoft.Azure.Common.Authentication;
using Microsoft.Azure.Documents;

namespace Microsoft.WindowsAzure.Commands.Test.DocumentDb
{
    public class GetDocumentDbDatabaseTest : TestBase
    {
        Mock<IDocumentDbClientExtensions> documentDbClientExtensionsMock;
        MockCommandRuntime mockCommandRuntime;
        GetAzureDocumentDbDatabaseCommand cmdlet;

        public GetDocumentDbDatabaseTest()
        {
            new FileSystemHelper(this).CreateAzureSdkDirectoryAndImportPublishSettings();
            documentDbClientExtensionsMock = new Mock<IDocumentDbClientExtensions>();
            mockCommandRuntime = new MockCommandRuntime();
            cmdlet = new GetAzureDocumentDbDatabaseCommand()
            {
                CommandRuntime = mockCommandRuntime,
                DocumentDbClient = documentDbClientExtensionsMock.Object,
                ServiceEndpoint = "https://sample-service-endpoint.documents.azure.com:443/",
                AuthKey = "LqW32nIIAhvLhrBpsYpN4lZ9cXIp+f3XeXtLrxy/M9dkllN0nrBKpnthT15Gn/gDob5SPMxpgaIoM5l69B51mg=="
            };
            AzureSession.AuthenticationFactory = new MockTokenAuthenticationFactory();
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void GetDocumentDbDatabaseByIdSuccessful()
        {
            // Setup
            cmdlet.Id = "test-db";
            var expected = new ExtendedDocumentDbDatabase { Id = cmdlet.Id };
            documentDbClientExtensionsMock.Setup(m => m.GetDatabase(cmdlet.Id)).Returns(expected);

            // Test
            cmdlet.ExecuteCmdlet();

            // Assert
            var actual = mockCommandRuntime.OutputPipeline[0] as Database;
            Assert.Equal<string>(expected.Id, actual.Id);
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void GetDocumentDbDatabasesSuccessful()
        {
            // Setup
            var expected = new List<ExtendedDocumentDbDatabase>() { new ExtendedDocumentDbDatabase { Id = "test-db-1" }, new ExtendedDocumentDbDatabase { Id = "test-db-2" } };
            documentDbClientExtensionsMock.Setup(m => m.GetDatabases()).Returns(expected);
            cmdlet.Id = String.Empty;

            // Test
            cmdlet.ExecuteCmdlet();

            // Assert
            IEnumerable<Database> actual = LanguagePrimitives.GetEnumerable(mockCommandRuntime.OutputPipeline).Cast<Database>();

            Assert.NotNull(actual);
            Assert.Equal<int>(expected.Count, actual.Count());
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.True(actual.Any((space) => space.Id == expected[i].Id));
            }
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void GetDocumentDbDatabaseByIdFails()
        {
            // Setup
            cmdlet.Id = "test-db";
            documentDbClientExtensionsMock.Setup(m => m.GetDatabase(It.IsAny<string>())).Returns<Database>(null); // The specified DocumentDb Database cannot be found

            // Test
            Testing.AssertThrows<Exception>(() => cmdlet.ExecuteCmdlet(), String.Format("Could not locate DocumentDb Database with the Id '{0}'", cmdlet.Id));
        }
    }
}

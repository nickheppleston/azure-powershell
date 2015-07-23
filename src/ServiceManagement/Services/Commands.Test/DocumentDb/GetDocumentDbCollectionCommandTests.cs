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
using Microsoft.WindowsAzure.Commands.Common.Test.Mocks;
using Microsoft.WindowsAzure.Commands.DocumentDb;
using Microsoft.WindowsAzure.Commands.Test.Utilities.Common;
using Microsoft.WindowsAzure.Commands.Utilities.DocumentDb;
using Microsoft.Azure.Common.Authentication;
using Microsoft.Azure.Documents;
using Xunit;
using Moq;

namespace Microsoft.WindowsAzure.Commands.Test.DocumentDb
{
    public class GetDocumentDbCollectionCommandTests : TestBase
    {
        Mock<IDocumentDbClientExtensions> documentDbClientExtensionsMock;
        MockCommandRuntime mockCommandRuntime;
        GetAzureDocumentDbCollectionCommand cmdlet;

        public GetDocumentDbCollectionCommandTests()
        {
            new FileSystemHelper(this).CreateAzureSdkDirectoryAndImportPublishSettings();
            documentDbClientExtensionsMock = new Mock<IDocumentDbClientExtensions>();
            mockCommandRuntime = new MockCommandRuntime();
            cmdlet = new GetAzureDocumentDbCollectionCommand()
            {
                CommandRuntime = mockCommandRuntime,
                DocumentDbClient = documentDbClientExtensionsMock.Object,
                ServiceEndpoint = "https://sample-service-endpoint.documents.azure.com:443/",
                AuthKey = "LqW32nIIAhvLhrBpsYpN4lZ9cXIp+f3XeXtLrxy/M9dkllN0nrBKpnthT15Gn/gDob5SPMxpgaIoM5l69B51mg==",
                CollectionsLink = "dbs/J6kkAA==/colls/",
                Id = "expected-collection"
            };
            AzureSession.AuthenticationFactory = new MockTokenAuthenticationFactory();
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void GetDocumentDbCollectionById_ReturnsDocumentCollection()
        {
            // Setup
            var expected = DocumentDbFactory.CreateExtendedDocumentDbCollection(cmdlet.Id);
            documentDbClientExtensionsMock.Setup(m => m.GetDocumentCollection(cmdlet.CollectionsLink, cmdlet.Id)).Returns(expected);

            // Test
            cmdlet.ExecuteCmdlet();

            // Assert
            var actual = mockCommandRuntime.OutputPipeline[0] as ExtendedDocumentDbCollection;
            Assert.Equal<String>(expected.ETag, actual.ETag);
            Assert.Equal<String>(expected.Id, actual.Id);
            Assert.Equal<String>(expected.ResourceId, actual.ResourceId);
            Assert.Equal<String>(expected.SelfLink, actual.SelfLink);
            Assert.Equal<DateTime>(expected.Timestamp, actual.Timestamp);
            Assert.Null(actual.IndexingPolicy);
            Assert.Equal<String>(expected.ConflictsLink, actual.ConflictsLink);
            Assert.Equal<String>(expected.DocumentsLink, actual.DocumentsLink);
            Assert.Equal<String>(expected.StoredProceduresLink, actual.StoredProceduresLink);
            Assert.Equal<String>(expected.UserDefinedFunctionsLink, actual.UserDefinedFunctionsLink);
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void GetDocumentDbCollectionById_UnknownCollection_ReturnsWarning()
        {
            // Setup
            documentDbClientExtensionsMock.Setup(m => m.GetDocumentCollection(cmdlet.CollectionsLink, cmdlet.Id)).Returns<ExtendedDocumentDbDatabase>(null); // The specified DocumentDb Database cannot be found

            // Test
            cmdlet.ExecuteCmdlet();

            // Assert
            var actual = mockCommandRuntime.WarningStream[0];
            Assert.False(String.IsNullOrEmpty(actual));
            Assert.Equal<String>(String.Format("Could not locate DocumentDb Document Collection with the CollectionsLink '{0}' and Id '{1}'", cmdlet.CollectionsLink, cmdlet.Id), actual);
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void GetDocumentDbCollections_ReturnsListOfCollections()
        {
            // Setup
            var expected = new List<ExtendedDocumentDbCollection>() { DocumentDbFactory.CreateExtendedDocumentDbCollection("collection-1"), DocumentDbFactory.CreateExtendedDocumentDbCollection("collection-2") };
            documentDbClientExtensionsMock.Setup(m => m.GetDocumentCollections(cmdlet.CollectionsLink)).Returns(expected);
            cmdlet.Id = String.Empty;

            // Test
            cmdlet.ExecuteCmdlet();

            // Assert
            var actual = LanguagePrimitives.GetEnumerable(mockCommandRuntime.OutputPipeline).Cast<ExtendedDocumentDbCollection>();

            Assert.NotNull(actual);
            Assert.Equal<int>(expected.Count, actual.Count());
            for (int i = 0; i < expected.Count; i++)
            {
                var actualCollection = actual.Where(col => col.Id == expected[i].Id).Select(col => col).SingleOrDefault();

                Assert.Equal<String>(expected[i].ETag, actualCollection.ETag);
                Assert.Equal<String>(expected[i].Id, actualCollection.Id);
                Assert.Equal<String>(expected[i].ResourceId, actualCollection.ResourceId);
                Assert.Equal<String>(expected[i].SelfLink, actualCollection.SelfLink);
                Assert.Equal<DateTime>(expected[i].Timestamp, actualCollection.Timestamp);
                Assert.Null(actualCollection.IndexingPolicy);
                Assert.Equal<String>(expected[i].ConflictsLink, actualCollection.ConflictsLink);
                Assert.Equal<String>(expected[i].DocumentsLink, actualCollection.DocumentsLink);
                Assert.Equal<String>(expected[i].StoredProceduresLink, actualCollection.StoredProceduresLink);
                Assert.Equal<String>(expected[i].UserDefinedFunctionsLink, actualCollection.UserDefinedFunctionsLink);
            }
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void GetDocumentDbCollections_ReturnsEmptyListOfCollections()
        {
            // Setup
            documentDbClientExtensionsMock.Setup(m => m.GetDocumentCollections(cmdlet.CollectionsLink)).Returns(new List<ExtendedDocumentDbCollection>()); // The specified DocumentDb Collection cannot be found

            // Test
            cmdlet.ExecuteCmdlet();

            // Assert
            IEnumerable<ExtendedDocumentDbCollection> actual = LanguagePrimitives.GetEnumerable(mockCommandRuntime.OutputPipeline).Cast<ExtendedDocumentDbCollection>();

            Assert.NotNull(actual);
            Assert.Equal<int>(0, actual.Count());
        }
    }
}

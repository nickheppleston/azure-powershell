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
using System.Management.Automation;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Microsoft.WindowsAzure.Commands.Utilities.DocumentDb;

namespace Microsoft.WindowsAzure.Commands.DocumentDb
{
    /// <summary>
    /// Gets a DocumentDb Collection/s
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureDocumentDbCollection"), OutputType(typeof(List<ExtendedDocumentDbCollection>), typeof(ExtendedDocumentDbCollection))]
    public class GetAzureDocumentDbCollectionCommand : AzurePSCmdlet
    {
        internal IDocumentDbClientExtensions DocumentDbClient { get; set; }

        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Service Endpoint Url of the DocumentDb account")]
        public string ServiceEndpoint { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Authorization Key to use to access the DocumentDb account")]
        public string AuthKey { get; set; }

        [Parameter(Position = 2, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The CollectionsLink from the referencing DocumentDb Database")]
        public string CollectionsLink { get; set; }

        [Parameter(Position = 3, Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "The Id of the Collection")]
        public string Id { get; set; }

        /// <summary>
        /// Executes the cmdlet.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            DocumentDbClient = DocumentDbClient ?? new DocumentDbClientExtensions(ServiceEndpoint, AuthKey);

            try
            {
                if (String.IsNullOrEmpty(Id))
                {
                    WriteObject(DocumentDbClient.GetDocumentCollections(CollectionsLink), true);
                }
                else
                {
                    var collection = DocumentDbClient.GetDocumentCollection(CollectionsLink, Id);

                    if (collection == null)
                        WriteWarning(String.Format("Could not locate DocumentDb Document Collection with the CollectionsLink '{0}' and Id '{1}'", CollectionsLink, Id));
                    else
                        WriteObject(collection);
                }
            }
            catch (AggregateException aggregateException)
            {
                aggregateException.Handle((ex) =>
                {
                    throw new Exception(String.Format("Error executing the command: {0}", ex.Message));
                });
            }
        }
    }
}
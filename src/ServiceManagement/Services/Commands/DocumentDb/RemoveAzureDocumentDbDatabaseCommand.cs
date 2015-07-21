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
using System.Management.Automation;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Microsoft.WindowsAzure.Commands.Utilities.DocumentDb;
using Microsoft.WindowsAzure.Commands.Utilities.Properties;

namespace Microsoft.WindowsAzure.Commands.DocumentDb
{
    /// <summary>
    /// Deletes a DocumentDb Database
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "AzureDocumentDbDatabase", SupportsShouldProcess = true), OutputType(typeof(bool))]
    public class RemoveAzureDocumentDbDatabaseCommand : AzurePSCmdlet
    {
        internal IDocumentDbClientExtensions DocumentDbClient { get; set; }

        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Service Endpoint Url of the DocumentDb account")]
        public string ServiceEndpoint { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Authorization Key to use to access the DocumentDb account")]
        public string AuthKey { get; set; }

        [Parameter(Position = 2, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The Id of the Database")]
        public string Id { get; set; }

        [Parameter(Position = 3, Mandatory = false)]
        public SwitchParameter PassThru { get; set; }

        [Parameter(Position = 4, HelpMessage = "Do not confirm the removal of the DocumentDb Database")]
        public SwitchParameter Force { get; set; }

        /// <summary>
        /// Executes the cmdlet.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            DocumentDbClient = DocumentDbClient ?? new DocumentDbClientExtensions(ServiceEndpoint, AuthKey);

            var database = DocumentDbClient.GetDatabase(Id);

            if (database == null)
            {
                WriteWarning(String.Format("Could not locate DocumentDb Database with the Id '{0}'", Id));
            }
            else
            {
                try
                {
                    ConfirmAction(
                        Force.IsPresent,
                        String.Format(Resources.RemoveDocumentDbDatabaseConfirmation, Id),
                        String.Format(Resources.RemovingDocumentDbDatabaseMessage),
                        Id,
                        async () =>
                        {
                            try
                            {
                                await DocumentDbClient.DeleteDatabaseAsync(database.SelfLink);

                                if (PassThru)
                                {
                                    WriteObject(true);
                                }
                            }
                            catch (AggregateException aggregateException)
                            {
                                aggregateException.Handle((ex) =>
                                {
                                    throw new Exception(String.Format("Error executing the command: {0}", ex.Message));
                                });
                            }
                        });
                }
                catch (AggregateException aggregateException)
                {
                    aggregateException.Handle((ex) =>
                    {
                        throw new Exception(ex.Message);
                    });
                }
            }
        }
    }
}
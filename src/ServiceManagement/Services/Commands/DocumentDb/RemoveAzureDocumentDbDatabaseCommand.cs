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
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Microsoft.WindowsAzure.Commands.Utilities.ServiceBus;
using Microsoft.WindowsAzure.Commands.Utilities.DocumentDb;
using Microsoft.WindowsAzure.Commands.Utilities.Properties;

namespace Microsoft.WindowsAzure.Commands.DocumentDb
{
    /// <summary>
    /// Gets a DocumentDb Database/s
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

        [Parameter(Position = 4, HelpMessage = "Do not confirm the removal of the namespace")]
        public SwitchParameter Force { get; set; }

        /// <summary>
        /// Executes the cmdlet.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            DocumentDbClient = DocumentDbClient ?? new DocumentDbClientExtensions(ServiceEndpoint, AuthKey);

            var database = DocumentDbClient.GetDatabase(Id);

            if (database == null)
                throw new Exception(String.Format("Could not locate DocumentDb Database with the Id '{0}'", Id));

            ConfirmAction(
                Force.IsPresent,
                String.Format(Resources.RemoveDocumentDbDatabaseConfirmation, Id),
                String.Format(Resources.RemovingDocumentDbDatabaseMessage),
                Id,
                async () =>
                {
                    await DocumentDbClient.DeleteDatabaseAsync(database.SelfLink);

                    if (PassThru)
                    {
                        WriteObject(true);
                    }
                });
        }
    }
}

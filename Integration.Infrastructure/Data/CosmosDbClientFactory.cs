// <copyright file="CosmosDbClientFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Integration.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Integration.Infrastructure.Contracts;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;

    /// <summary>
    /// CosmosDbClientFactory class
    /// </summary>
    public class CosmosDbClientFactory : ICosmosDbClientFactory
    {
        private readonly string databaseName;
        private readonly List<string> collectionNames;
        private readonly IDocumentClient documentClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDbClientFactory"/> class.
        /// </summary>
        /// <param name="databaseName">database name</param>
        /// <param name="collectionNames">collection names</param>
        /// <param name="documentClient">document client</param>
        public CosmosDbClientFactory(string databaseName, List<string> collectionNames, IDocumentClient documentClient)
        {
            this.databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            this.collectionNames = collectionNames ?? throw new ArgumentNullException(nameof(collectionNames));
            this.documentClient = documentClient ?? throw new ArgumentNullException(nameof(documentClient));
        }

        /// <summary>
        /// Get Client method
        /// </summary>
        /// <param name="collectionName">collection name</param>
        /// <returns>the comosdb Client</returns>
        public ICosmosDbClient GetClient(string collectionName)
        {
            if (!this.collectionNames.Contains(collectionName))
            {
                throw new ArgumentException($"Unable to find collection: {collectionName}");
            }

            return new CosmosDbClient(this.databaseName, collectionName, this.documentClient);
        }

        /// <summary>
        /// EnsureDbSetup Async
        /// </summary>
        /// <returns>the task</returns>
        public async Task EnsureDbSetupAsync()
        {
            await this.documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(this.databaseName)).ConfigureAwait(true);

            foreach (var collectionName in this.collectionNames)
            {
                await this.documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(this.databaseName, collectionName)).ConfigureAwait(true);
            }
        }
    }
}

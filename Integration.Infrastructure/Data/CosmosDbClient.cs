// <copyright file="CosmosDbClient.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Integration.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Integration.Infrastructure.Contracts;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    /// <summary>
    /// CosmosDbClient class
    /// </summary>
    public class CosmosDbClient : ICosmosDbClient
    {
        private readonly string databaseName;
        private readonly string collectionName;
        private readonly IDocumentClient documentClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDbClient"/> class.
        /// </summary>
        /// <param name="databaseName">database name</param>
        /// <param name="collectionName">collection name</param>
        /// <param name="documentClient">document client</param>
        public CosmosDbClient(string databaseName, string collectionName, IDocumentClient documentClient)
        {
            this.databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            this.collectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            this.documentClient = documentClient ?? throw new ArgumentNullException(nameof(documentClient));
        }

        /// <summary>
        /// Read Document Async
        /// </summary>
        /// <param name="documentId">document Id</param>
        /// <param name="options">options</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>the document</returns>
        public async Task<Document> ReadDocumentAsync(string documentId, RequestOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.documentClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(this.databaseName, this.collectionName, documentId), options, cancellationToken).ConfigureAwait(true);
        }

#pragma warning disable CA1715 // Identifiers should have correct prefix
        /// <summary>
        /// Read Document Async
        /// </summary>
        /// <typeparam name="dynamic">type param</typeparam>
        /// <param name="predicate">predicate</param>
        /// <param name="feedOptions">options</param>
        /// <returns>the document</returns>
        public async Task<IEnumerable<dynamic>> QueryDocumentAsync<dynamic>(Expression<Func<dynamic, bool>> predicate, FeedOptions feedOptions = null)
#pragma warning restore CA1715 // Identifiers should have correct prefix
        {
            var query = this.documentClient.CreateDocumentQuery<dynamic>(UriFactory.CreateDocumentCollectionUri(this.databaseName, this.collectionName), feedOptions).Where(predicate).AsDocumentQuery();
            var results = new List<dynamic>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<dynamic>().ConfigureAwait(true));
            }

            return results;
        }

        /// <summary>
        /// Create DocumentAsync
        /// </summary>
        /// <param name="document">document</param>
        /// <param name="options">options</param>
        /// <param name="disableAutomaticIdGeneration">Disable Automatic Id generation</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>the document</returns>
        public async Task<Document> CreateDocumentAsync(object document, RequestOptions options = null, bool disableAutomaticIdGeneration = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(this.databaseName, this.collectionName), document, options, disableAutomaticIdGeneration, cancellationToken).ConfigureAwait(true);
        }

        /// <summary>
        /// Replace the document
        /// </summary>
        /// <param name="documentId">the document id</param>
        /// <param name="document">document</param>
        /// <param name="options">options</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>The document</returns>
        public async Task<Document> ReplaceDocumentAsync(string documentId, object document, RequestOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(this.databaseName, this.collectionName, documentId), document, options, cancellationToken).ConfigureAwait(true);
        }

        /// <summary>
        /// Delete the documents
        /// </summary>
        /// <param name="documentId">the document id</param>
        /// <param name="options">the options</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>The document</returns>
        public async Task<Document> DeleteDocumentAsync(string documentId, RequestOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.documentClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(this.databaseName, this.collectionName, documentId), options, cancellationToken).ConfigureAwait(true);
        }
    }
}

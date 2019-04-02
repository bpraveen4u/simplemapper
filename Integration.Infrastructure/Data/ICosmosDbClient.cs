// <copyright file="ICosmosDbClient.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Integration.Infrastructure.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;

    /// <summary>
    /// ICosmosDbClient interface
    /// </summary>
    public interface ICosmosDbClient
    {
        /// <summary>
        /// Read Document Async
        /// </summary>
        /// <param name="documentId">document Id</param>
        /// <param name="options">options</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>the document</returns>
        Task<Document> ReadDocumentAsync(string documentId, RequestOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Read Document Async
        /// </summary>
        /// <typeparam name="T">type param</typeparam>
        /// <param name="predicate">predicate</param>
        /// <param name="feedOptions">options</param>
        /// <returns>the document</returns>
        Task<IEnumerable<T>> QueryDocumentAsync<T>(Expression<Func<T, bool>> predicate, FeedOptions feedOptions = null);

        /// <summary>
        /// Create DocumentAsync
        /// </summary>
        /// <param name="document">document</param>
        /// <param name="options">options</param>
        /// <param name="disableAutomaticIdGeneration">Disable Automatic Id generation</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>the document</returns>
        Task<Document> CreateDocumentAsync(object document, RequestOptions options = null, bool disableAutomaticIdGeneration = false, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Replace the document
        /// </summary>
        /// <param name="documentId">the document id</param>
        /// <param name="document">document</param>
        /// <param name="options">options</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>The document</returns>
        Task<Document> ReplaceDocumentAsync(string documentId, object document, RequestOptions options = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Delete the documents
        /// </summary>
        /// <param name="documentId">the document id</param>
        /// <param name="options">the options</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>The document</returns>
        Task<Document> DeleteDocumentAsync(string documentId, RequestOptions options = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}

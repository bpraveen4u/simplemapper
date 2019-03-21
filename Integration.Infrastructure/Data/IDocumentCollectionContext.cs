// <copyright file="IDocumentCollectionContext.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Integration.Infrastructure.Contracts
{
    using Microsoft.Azure.Documents;
    using Microsoft.Integration.Mapper.Contracts.Entities;

    /// <summary>
    /// IDocumentCollectionContext interface
    /// </summary>
    /// <typeparam name="T">the entity</typeparam>
    public interface IDocumentCollectionContext<in T>
        where T : Entity
    {
        /// <summary>
        /// Gets the collection name
        /// </summary>
        string CollectionName { get; }

        /// <summary>
        /// Generate the id
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <returns>guid</returns>
        string GenerateId(T entity);

        /// <summary>
        /// Resolve PartitionKey
        /// </summary>
        /// <param name="entityId">the entity id</param>
        /// <returns>Resolved PartitionKey</returns>
        PartitionKey ResolvePartitionKey(string entityId);
    }
}

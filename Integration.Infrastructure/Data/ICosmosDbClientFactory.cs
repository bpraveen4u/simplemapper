// <copyright file="ICosmosDbClientFactory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Integration.Infrastructure.Contracts
{
    /// <summary>
    /// ICosmosDbClientFactory interface
    /// </summary>
    public interface ICosmosDbClientFactory
    {
        /// <summary>
        /// Get Client method
        /// </summary>
        /// <param name="collectionName">collection name</param>
        /// <returns>the document client</returns>
        ICosmosDbClient GetClient(string collectionName);
    }
}

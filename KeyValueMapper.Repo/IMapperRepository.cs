// <copyright file="IMapperRepository.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Repo
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Mapper Repository
    /// </summary>
    /// <typeparam name="T">Type parameter</typeparam>
    public interface IMapperRepository<T>
    {
        /// <summary>
        /// Get the values by partition and key
        /// </summary>
        /// <param name="partition">partition key</param>
        /// <param name="key">row key</param>
        /// <returns>the value</returns>
        Task<T> GetByKey(string partition, string key);

        /// <summary>
        /// Get the keys by partition and value
        /// </summary>
        /// <param name="partition">partition key</param>
        /// <param name="value">the value</param>
        /// <returns>value/key combinations</returns>
        Task<List<T>> GetByValue(string partition, string value);

        /// <summary>
        /// Saves the mappings in persistance store
        /// </summary>
        /// <param name="mapping">the mapping object</param>
        /// <returns>The save object</returns>
        Task<T> Save(T mapping);

        /// <summary>
        /// Get the count
        /// </summary>
        /// <param name="partitionKey">partition key</param>
        /// <returns>count of mappings in the partition</returns>
        Task<int> Count(string partitionKey);
    }
}

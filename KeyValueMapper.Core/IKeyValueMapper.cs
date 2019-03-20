// <copyright file="IKeyValueMapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Integration.Mapper.Contracts.Models;

    /// <summary>
    /// IKeyValueMapper interface
    /// </summary>
    public interface IKeyValueMapper
    {
        /// <summary>
        /// Get mappings by key
        /// </summary>
        /// <param name="partition">partition key</param>
        /// <param name="key">row key</param>
        /// <returns>the mapping</returns>
        Task<Mapping> GetByKey(string partition, string key);

        /// <summary>
        /// Get mappings by value
        /// </summary>
        /// <param name="partition">partition key</param>
        /// <param name="value">value </param>
        /// <returns>the mappings</returns>
        Task<List<Mapping>> GetByValue(string partition, string value);

        /// <summary>
        /// Save the mappings
        /// </summary>
        /// <param name="mapping">the mapping</param>
        /// <returns>save mappings</returns>
        Task<Mapping> Save(Mapping mapping);

        /// <summary>
        /// The count by partition
        /// </summary>
        /// <param name="partitionKey">partition key</param>
        /// <returns>the count</returns>
        Task<int> Count(string partitionKey);
    }
}

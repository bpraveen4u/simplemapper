// <copyright file="KeyValueMapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Integration.Mapper.Contracts.Models;
    using Microsoft.Integration.Mapper.Repo;

    /// <summary>
    /// The key value mapper
    /// </summary>
    public class KeyValueMapper : IKeyValueMapper
    {
        private readonly IConfiguration config;
        private readonly IMapperRepository<Mapping> mapperRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueMapper"/> class.
        /// </summary>
        /// <param name="config">config object</param>
        /// <param name="mapperRepository">repository object</param>
        public KeyValueMapper(IConfiguration config, IMapperRepository<Mapping> mapperRepository)
        {
            this.config = config;
            this.mapperRepository = mapperRepository;
        }

        /// <summary>
        /// The count by partition
        /// </summary>
        /// <param name="partitionKey">the partition key</param>
        /// <returns>the count of values</returns>
        public Task<int> Count(string partitionKey)
        {
            return this.mapperRepository.Count(partitionKey);
        }

        /// <summary>
        /// Get Mapings By Key
        /// </summary>
        /// <param name="partition">the partition</param>
        /// <param name="key">the key</param>
        /// <returns>the mappings</returns>
        public async Task<Mapping> GetByKey(string partition, string key)
        {
            var allowedPartitions = this.config["AppSettings:AllowedPartitions"].Split(',');
            if (!allowedPartitions.Any(p => p.Equals(partition, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            return await this.mapperRepository.GetByKey(partition.ToLowerInvariant(), key).ConfigureAwait(true);
        }

        /// <summary>
        /// Get Mapings By value
        /// </summary>
        /// <param name="partition">the partition</param>
        /// <param name="value">the value</param>
        /// <returns>the mappings</returns>
        public async Task<List<Mapping>> GetByValue(string partition, string value)
        {
            var allowedPartitions = this.config["AppSettings:AllowedPartitions"].Split(',');
            if (!allowedPartitions.Any(p => p.Equals(partition, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            return await this.mapperRepository.GetByValue(partition.ToLowerInvariant(), value).ConfigureAwait(true);
        }

        /// <summary>
        /// Save the mapping
        /// </summary>
        /// <param name="mapping">the mapping</param>
        /// <returns>the save mapping</returns>
        public async Task<Mapping> Save(Mapping mapping)
        {
            var allowedPartitions = this.config["AppSettings:AllowedPartitions"].Split(',');
            if (!allowedPartitions.Any(p => p.Equals(mapping.PartitionKey, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            return await this.mapperRepository.Save(mapping).ConfigureAwait(true);
        }
    }
}

// <copyright file="KeyValueMapper.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Integration.Mapper.Contracts.Models;
    using Microsoft.Integration.Mapper.Contracts.Repo;
    using Microsoft.Integration.Mapper.Contracts.Service;

    /// <summary>
    /// The key value mapper
    /// </summary>
    public class KeyValueMapper : IKeyValueMapper
    {
        private readonly IConfiguration config;
        private readonly IKeyValueMapperRepository mapperRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueMapper"/> class.
        /// </summary>
        /// <param name="config">config object</param>
        /// <param name="mapperRepository">repository object</param>
        public KeyValueMapper(IConfiguration config, IKeyValueMapperRepository mapperRepository)
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
            return Task.FromResult(0); //// this.mapperRepository.Count(partitionKey);
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

            var result = await this.mapperRepository.GetItemsAsync(t => t.Category == partition && t.Name == key).ConfigureAwait(true);
            if (result != null && result.Count() > 0)
            {
                return new Mapping() { PartitionKey = result.First().Category, RowKey = result.First().Name, Value = result.First().Value };
            }

            return null;
        }

        /// <summary>
        /// Get Mapings By value
        /// </summary>
        /// <param name="partition">the partition</param>
        /// <param name="value">the value</param>
        /// <returns>the mappings</returns>
        public async Task<List<Mapping>> GetByValue(string partition, string value)
        {
            var defaultResult = new List<Mapping> { new Mapping() { PartitionKey = partition, RowKey = string.Empty, Value = value } };
            var allowedPartitions = this.config["AppSettings:AllowedPartitions"].Split(',');
            if (!allowedPartitions.Any(p => p.Equals(partition, StringComparison.OrdinalIgnoreCase)))
            {
                return defaultResult;
            }

            var result = await this.mapperRepository.GetItemsAsync(t => t.Category == partition && t.Value == value).ConfigureAwait(true);
            if (result != null && result.Count() > 0)
            {
                return result.Select(r => new Mapping() { PartitionKey = r.Category, RowKey = r.Name, Value = r.Value }).ToList();
            }

            return defaultResult;
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

            var entity = new Contracts.Entities.KeyValueEntity() { Category = mapping.PartitionKey, Name = mapping.RowKey, Value = mapping.Value };
            var result = await this.mapperRepository.AddAsync(entity).ConfigureAwait(true);
            return new Mapping() { PartitionKey = result.Category, RowKey = result.Name, Value = result.Value };
        }
    }
}

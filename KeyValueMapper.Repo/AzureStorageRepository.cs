// <copyright file="AzureStorageRepository.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Repo
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Integration.Mapper.Contracts.Repo;
    using Microsoft.Integration.Mapper.Repo.Entities;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json;

    /// <summary>
    /// Azure Storage Repository
    /// </summary>
    /// <typeparam name="T">type parameter</typeparam>
    public class AzureStorageRepository<T> : IMapperRepository<T>
    {
        private readonly IConfiguration config;
        private readonly string storageConnectionString;
        private readonly string storageTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageRepository{T}"/> class.
        /// </summary>
        /// <param name="config">the configuration instance</param>
        public AzureStorageRepository(IConfiguration config)
        {
            this.config = config;
            this.storageConnectionString = config["ConnectionStrings:AzureStorageConnectionString"];
            this.storageTable = config["AppSettings:StorageTableName"];
        }

        /// <summary>
        /// The count of records
        /// </summary>
        /// <param name="partitionKey">the partition key</param>
        /// <returns>count of records</returns>
        public async Task<int> Count(string partitionKey)
        {
            TableContinuationToken token = null;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.storageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(this.storageTable);

            var query = new TableQuery<MapperStorageEntity>()
#pragma warning disable CA1308 // Normalize strings to uppercase
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey.ToLowerInvariant()))
#pragma warning restore CA1308 // Normalize strings to uppercase
                .Select(new List<string> { "PartitionKey", "RowKey", "Timestamp" });

            var result = await table.ExecuteQuerySegmentedAsync(query, token).ConfigureAwait(true);
            return result.Results.Count();
        }

        /// <summary>
        /// Get mappings by Key
        /// </summary>
        /// <param name="partition">the partition</param>
        /// <param name="key">the key</param>
        /// <returns>the mappings</returns>
        public async Task<T> GetByKey(string partition, string key)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.storageConnectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(this.storageTable);
                MapperStorageEntity mapperStorageEntity;

                var columns = new List<string> { nameof(mapperStorageEntity.Value) };
                TableOperation retrieveOperation = TableOperation.Retrieve<MapperStorageEntity>(partition, key, columns);
                TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation).ConfigureAwait(true);
                mapperStorageEntity = retrievedResult.Result as MapperStorageEntity;

                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(mapperStorageEntity));
            }
            catch (StorageException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the mapping by value
        /// </summary>
        /// <param name="partition">the partionkey</param>
        /// <param name="value">the value</param>
        /// <returns>the mappings</returns>
        public async Task<List<T>> GetByValue(string partition, string value)
        {
            TableContinuationToken token = null;

            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.storageConnectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(this.storageTable);

                TableQuery<MapperStorageEntity> rangeQuery = new TableQuery<MapperStorageEntity>().Where(
                                                              TableQuery.CombineFilters(
                                                              TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partition),
                                                              TableOperators.And,
                                                              TableQuery.GenerateFilterCondition("Value", QueryComparisons.Equal, value)));

                var results = await table.ExecuteQuerySegmentedAsync(rangeQuery, token).ConfigureAwait(true);

                return JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(results.Results));
            }
            catch (StorageException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Saves the mapping
        /// </summary>
        /// <param name="mapping">the mapping object</param>
        /// <returns>the saved object</returns>
        public async Task<T> Save(T mapping)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.storageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(this.storageTable);

            // Retrieve a reference to cloud tables by providing table names
            CloudTable messagesStoreTable = tableClient.GetTableReference(this.storageTable.ToString(CultureInfo.InvariantCulture));
            var mappingEntity = JsonConvert.DeserializeObject<MapperStorageEntity>(JsonConvert.SerializeObject(mapping));

            try
            {
                await messagesStoreTable.ExecuteAsync(TableOperation.Insert(mappingEntity)).ConfigureAwait(true);
                return mapping;
            }
            catch (StorageException se)
            {
                // in case of concurrent operations, handle the conflict of same row already added concurrently
                if ((HttpStatusCode)se.RequestInformation.HttpStatusCode == HttpStatusCode.Conflict)
                {
                    return default(T);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

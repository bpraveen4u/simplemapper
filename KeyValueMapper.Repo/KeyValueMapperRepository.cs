// <copyright file="KeyValueMapperRepository.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Repo
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Integration.Infrastructure.Contracts;
    using global::Integration.Infrastructure.Data;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Integration.Mapper.Contracts.Entities;
    using Microsoft.Integration.Mapper.Contracts.Repo;

    /// <summary>
    /// Cosmos DB Storage Repository
    /// </summary>
    public class KeyValueMapperRepository : CosmosDbRepository<KeyValueEntity>, IKeyValueMapperRepository
    {
        private readonly IConfiguration config;
        private readonly string collectionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValueMapperRepository"/> class.
        /// </summary>
        /// <param name="factory">the cosmos db factory</param>
        /// <param name="config">the config</param>
        public KeyValueMapperRepository(ICosmosDbClientFactory factory, IConfiguration config)
            : base(factory)
        {
            ////this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);

            this.config = config;
            this.collectionName = this.config["CosmosDb:MetadataCollectionName"];
        }

        /// <summary>
        /// Gets the collection name
        /// </summary>
        public override string CollectionName => this.collectionName;
    }
}

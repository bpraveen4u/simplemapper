// <copyright file="ServiceCollectionCosmosDbExtensions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integraton.Mapper.Extensions
{
    using System;
    using System.Collections.Generic;
    using global::Integration.Infrastructure.Contracts;
    using global::Integration.Infrastructure.Data;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    /// <summary>
    /// ServiceCollectionCosmosDbExtensions class
    /// </summary>
    public static class ServiceCollectionCosmosDbExtensions
    {
        /// <summary>
        /// AddCosmosDb method
        /// </summary>
        /// <param name="services">services collection</param>
        /// <param name="serviceEndpoint">service Endpoint</param>
        /// <param name="authKey">auth key</param>
        /// <param name="databaseName">database name</param>
        /// <param name="collectionNames">collection names</param>
        /// <returns>services builder</returns>
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, Uri serviceEndpoint, string authKey, string databaseName, List<string> collectionNames)
        {
            var documentClient = new DocumentClient(
                serviceEndpoint,
                authKey,
#pragma warning disable SA1118 // Parameter must not span multiple lines
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore, ////ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
#pragma warning restore SA1118 // Parameter must not span multiple lines

            documentClient.OpenAsync().Wait();

            var cosmosDbClientFactory = new CosmosDbClientFactory(databaseName, collectionNames, documentClient);
            cosmosDbClientFactory.EnsureDbSetupAsync().Wait();

            services.AddSingleton<ICosmosDbClientFactory>(cosmosDbClientFactory);

            return services;
        }
    }
}

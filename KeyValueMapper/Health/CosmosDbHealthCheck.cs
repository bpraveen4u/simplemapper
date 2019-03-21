// <copyright file="CosmosDbHealthCheck.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integraton.Mapper.Health
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Integration.Mapper.Contracts.Service;

    /// <summary>
    /// Storage Table Health Check
    /// </summary>
    public class CosmosDbHealthCheck : IHealthCheck
    {
        /// <summary>
        /// the Configuration
        /// </summary>
        private readonly IConfiguration config;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDbHealthCheck"/> class.
        /// </summary>
        /// <param name="keyValueMapper">the keyValueMapper</param>
        /// <param name="config">the config</param>
        public CosmosDbHealthCheck(IConfiguration config)
        {
            this.config = config;
        }

        /// <summary>
        /// Check Health Async
        /// </summary>
        /// <param name="context">the context</param>
        /// <param name="cancellationToken">the cancellation token</param>
        /// <returns>The health check result</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Execute health check logic here. This example sets a dummy
            // variable to true.
            var healthCheckResultHealthy = true;
            var serviceEndpoint = this.config["ConnectionStrings:ServiceEndpoint"];
            var authKey = this.config["ConnectionStrings:AuthKey"];
            var checkName = $"CosmosDbCheck({serviceEndpoint})";

            if (healthCheckResultHealthy)
            {
                try
                {
                    using (var documentClient = new DocumentClient(new Uri(serviceEndpoint), authKey))
                    {
                        await documentClient.OpenAsync().ConfigureAwait(true);
                        return HealthCheckResult.Healthy($"{checkName}: Healthy");
                    }
                }
                catch (Exception ex)
                {
                    // Failed to connect to CosmosDB.
                    return HealthCheckResult.Unhealthy($"{checkName}: Exception during check: ${ex.Message}");
                }
            }

            return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
        }
    }
}

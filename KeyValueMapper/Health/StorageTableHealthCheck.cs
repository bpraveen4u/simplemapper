// <copyright file="StorageTableHealthCheck.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.Integraton.Mapper.Health
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Integration.Mapper.Core;

    /// <summary>
    /// Storage Table Health Check
    /// </summary>
    public class StorageTableHealthCheck : IHealthCheck
    {
        /// <summary>
        /// Key Value Mapper
        /// </summary>
        private readonly IKeyValueMapper keyValueMapper;

        /// <summary>
        /// the Configuration
        /// </summary>
        private readonly IConfiguration config;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageTableHealthCheck"/> class.
        /// </summary>
        /// <param name="keyValueMapper">the keyValueMapper</param>
        /// <param name="config">the config</param>
        public StorageTableHealthCheck(IKeyValueMapper keyValueMapper, IConfiguration config)
        {
            this.keyValueMapper = keyValueMapper;
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

            if (healthCheckResultHealthy)
            {
                var allowedPartitions = this.config["AppSettings:AllowedPartitions"]?.Split(',');
                if (allowedPartitions.Count() > 0)
                {
                    var count = await this.keyValueMapper.Count(allowedPartitions.First()).ConfigureAwait(true);

                    if (count == 0 && allowedPartitions.Count() > 1)
                    {
                        count = await this.keyValueMapper.Count(allowedPartitions[1]).ConfigureAwait(true);
                    }

                    if (count > 0)
                    {
                        return HealthCheckResult.Healthy("Blob has the data and accessible.");
                    }
                }
                else
                {
                    return HealthCheckResult.Unhealthy("The AllowedPartitions config value missing.");
                }
            }

            return HealthCheckResult.Unhealthy("The check indicates an unhealthy result.");
        }
    }
}

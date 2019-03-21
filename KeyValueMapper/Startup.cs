// <copyright file="Startup.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Integration.Mapper.Contracts.Repo;
    using Microsoft.Integration.Mapper.Contracts.Service;
    using Microsoft.Integration.Mapper.Core;
    using Microsoft.Integration.Mapper.Repo;
    using Microsoft.Integraton.Mapper.Extensions;
    using Microsoft.Integraton.Mapper.Health;
    using Microsoft.Integraton.Mapper.Options;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">the configuration</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">the services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<CosmosDbHealthCheck>("cosmosDb")
                .AddCheck<StorageTableHealthCheck>("blob_storage_table_health_check", failureStatus: Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded, tags: new[] { "blob_storage_table_health_check" });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddTransient<IKeyValueMapper, KeyValueMapper>();
            services.AddScoped(typeof(IMapperRepository<>), typeof(AzureStorageRepository<>));

            var cosmosDbOptions = this.Configuration.GetSection("CosmosDb").Get<CosmosDbOptions>();
            var (databaseName, collectionData) = cosmosDbOptions;
            var collectionNames = collectionData.Select(c => c.Name).ToList();

            // Add CosmosDb. This verifies database and collections existence.
            services.AddCosmosDb(new Uri(this.Configuration["ConnectionStrings:ServiceEndpoint"]), this.Configuration["ConnectionStrings:AuthKey"], this.Configuration["CosmosDb:DatabaseName"], collectionNames);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Mapper API", Version = "v1" });
            });

            services.AddScoped<IKeyValueMapperRepository, KeyValueMapperRepository>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">the app</param>
        /// <param name="env">the env</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mapper API V1");
            });

            var options = new HealthCheckOptions()
            {
                ResponseWriter = async (c, r) =>
                {
                    c.Response.ContentType = "application/json";

                    var result = JsonConvert.SerializeObject(new
                    {
                        status = r.Status.ToString(),
                        errors = r.Entries.Select(e => new { key = e.Key, status = e.Value.Status.ToString(), description = e.Value.Description.ToString(System.Globalization.CultureInfo.InvariantCulture) })
                    });
                    await c.Response.WriteAsync(result);
                }
            };

            app.UseHealthChecks("/health", options);
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

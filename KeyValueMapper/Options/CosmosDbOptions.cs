// <copyright file="CosmosDbOptions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integraton.Mapper.Options
{
    using System.Collections.Generic;

    /// <summary>
    /// CosmosDb Options
    /// </summary>
    public class CosmosDbOptions
    {
        /// <summary>
        /// Gets or sets Database name
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets Database collection names
        /// </summary>
        public List<CollectionInfo> CollectionNames { get; set; }

        /// <summary>
        /// Deconstruct parameters
        /// </summary>
        /// <param name="databaseName">database Name</param>
        /// <param name="collectionNames">collection Names</param>
        public void Deconstruct(out string databaseName, out List<CollectionInfo> collectionNames)
        {
            databaseName = this.DatabaseName;
            collectionNames = this.CollectionNames;
        }
    }
}

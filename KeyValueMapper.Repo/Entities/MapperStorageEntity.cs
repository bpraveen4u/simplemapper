// <copyright file="MapperStorageEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Repo.Entities
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Mapper Storage Entity
    /// </summary>
    public class MapperStorageEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }
    }
}

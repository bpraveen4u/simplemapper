// <copyright file="KeyValueEntity.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Contracts.Entities
{
    /// <summary>
    /// Key Value Entity
    /// </summary>
    public class KeyValueEntity : Entity
    {
        /// <summary>
        /// Gets or sets Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Category
        /// </summary>
        public string Category { get; set; }
    }
}

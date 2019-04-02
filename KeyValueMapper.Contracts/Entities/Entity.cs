// <copyright file="Entity.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Contracts.Entities
{
    /// <summary>
    /// Entity
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        /// <example>5fe3fc2a-cbac-4df0-8031-fdca0f682989</example>
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}

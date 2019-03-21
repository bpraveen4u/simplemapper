// <copyright file="Mapping.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Contracts.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The Mapping Class
    /// </summary>
    public class Mapping
    {
        /// <summary>Gets or sets the partition key.</summary>
        /// <value>The partition key.</value>
        [Required(ErrorMessage = "PartitionKey is Required")]
        public string PartitionKey { get; set; }

        /// <summary>Gets or sets the row key.</summary>
        /// <value>The row key.</value>
        [Required(ErrorMessage = "RowKey is Required")]
        public string RowKey { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Required(ErrorMessage = "Value is Required")]
        public string Value { get; set; }
    }
}

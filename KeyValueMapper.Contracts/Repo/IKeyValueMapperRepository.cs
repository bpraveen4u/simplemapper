// <copyright file="IKeyValueMapperRepository.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Contracts.Repo
{
    using Microsoft.Integration.Mapper.Contracts.Entities;
    using Microsoft.Integration.Mapper.Contracts.Infra;

    /// <summary>
    /// IKeyValueMapperRepository interface
    /// </summary>
    public interface IKeyValueMapperRepository : IRepository<KeyValueEntity>
    {
    }
}

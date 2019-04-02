// <copyright file="IRepository.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace Microsoft.Integration.Mapper.Contracts.Infra
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Integration.Mapper.Contracts.Entities;

    /// <summary>
    /// The repository interface
    /// </summary>
    /// <typeparam name="T">the repository type parameter</typeparam>
    public interface IRepository<T>
        where T : Entity
    {
        /// <summary>
        /// GetById Async
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>the entity</returns>
        Task<T> GetByIdAsync(string id);

        /// <summary>
        /// Get Items Async
        /// </summary>
        /// <param name="predicate">the predicate</param>
        /// <returns>Items</returns>
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Add the entity
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <returns>added entity</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <returns>task</returns>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Delets the entity
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <returns>task</returns>
        Task DeleteAsync(T entity);
    }
}

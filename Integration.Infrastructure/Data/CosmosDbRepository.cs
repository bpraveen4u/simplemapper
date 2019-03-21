using Integration.Infrastructure.Contracts;
using Integration.Infrastructure.Exceptions;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Integration.Mapper.Contracts.Entities;
using Microsoft.Integration.Mapper.Contracts.Infra;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Infrastructure.Data
{
    /// <summary>
    /// The Cosmos Db Repository
    /// </summary>
    /// <typeparam name="T">the Entity</typeparam>
    public abstract class CosmosDbRepository<T> : IRepository<T>, IDocumentCollectionContext<T> where T : Entity
    {
        /// <summary>
        /// Cosmos DB factory
        /// </summary>
        private readonly ICosmosDbClientFactory _cosmosDbClientFactory;

        /// <summary>
        /// The Cosmos DB repository
        /// </summary>
        /// <param name="cosmosDbClientFactory"></param>
        protected CosmosDbRepository(ICosmosDbClientFactory cosmosDbClientFactory)
        {
            _cosmosDbClientFactory = cosmosDbClientFactory;
        }

        /// <summary>
        /// GetById Async
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>the entity</returns>
        public async Task<T> GetByIdAsync(string id)
        {
            try
            {
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                var document = await cosmosDbClient.ReadDocumentAsync(id, new RequestOptions
                {
                    PartitionKey = ResolvePartitionKey(id)
                });

                return JsonConvert.DeserializeObject<T>(document.ToString());
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new EntityNotFoundException();
                }

                throw;
            }
        }

        /// <summary>
        /// Add the entity
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <returns>added entity</returns>
        public async Task<T> AddAsync(T entity)
        {
            try
            {
                entity.Id = GenerateId(entity);
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                var document = await cosmosDbClient.CreateDocumentAsync(entity);
                return JsonConvert.DeserializeObject<T>(document.ToString());
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new EntityAlreadyExistsException();
                }

                throw;
            }
        }

        /// <summary>
        /// Updates the entity
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <returns>task</returns>
        public async Task UpdateAsync(T entity)
        {
            try
            {
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                await cosmosDbClient.ReplaceDocumentAsync(entity.Id, entity);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new EntityNotFoundException();
                }

                throw;
            }
        }

        /// <summary>
        /// Delets the entity
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <returns>task</returns>
        public async Task DeleteAsync(T entity)
        {
            try
            {
                var cosmosDbClient = _cosmosDbClientFactory.GetClient(CollectionName);
                await cosmosDbClient.DeleteDocumentAsync(entity.Id, new RequestOptions
                {
                    PartitionKey = ResolvePartitionKey(entity.Id)
                });
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new EntityNotFoundException();
                }

                throw;
            }
        }

        /// <summary>
        /// Gets the collection name
        /// </summary>
        public abstract string CollectionName { get; }

        /// <summary>
        /// The GenerateId method
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <returns>guid</returns>
        public virtual string GenerateId(T entity) => Guid.NewGuid().ToString();

        /// <summary>
        /// Resolve Partition Key
        /// </summary>
        /// <param name="entityId">the entityId</param>
        /// <returns>the partition key</returns>
        public virtual PartitionKey ResolvePartitionKey(string entityId) => null;
    }
}

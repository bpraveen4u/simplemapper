using Microsoft.Azure.Documents;
using Microsoft.Integration.Mapper.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Infrastructure.Contracts
{
    public interface IDocumentCollectionContext<in T> where T : Entity
    {
        string CollectionName { get; }

        string GenerateId(T entity);

        PartitionKey ResolvePartitionKey(string entityId);
    }
}

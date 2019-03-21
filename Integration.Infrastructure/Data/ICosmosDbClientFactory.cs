using System;
using System.Collections.Generic;
using System.Text;

namespace Integration.Infrastructure.Contracts
{
    public interface ICosmosDbClientFactory
    {
        ICosmosDbClient GetClient(string collectionName);
    }
}

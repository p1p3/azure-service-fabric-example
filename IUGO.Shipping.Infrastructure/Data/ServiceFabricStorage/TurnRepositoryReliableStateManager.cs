using System;
using IUGO.Shippings.Core;
using IUGO.Shippings.Infrastructure.Data.ServiceFabricStorage.DTOs;
using IUGO.Storage.AzureServiceFabric;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Shipping = IUGO.Shippings.Core.ShippingAggregate.Shipping;

namespace IUGO.Shippings.Infrastructure.Data.ServiceFabricStorage
{
    internal class ShippingRepositoryReliableStateManager : AggregateGenericRepositoryReliableStateManager<Shipping,DTOs.Shipping>,IShippingRepository
    {
        public ShippingRepositoryReliableStateManager(IReliableDictionary<Guid, DTOs.Shipping> aggreateStorage,
            ITransaction transaction) : base(aggreateStorage, transaction, new ShippingMapper())
        {
        }
    }
}

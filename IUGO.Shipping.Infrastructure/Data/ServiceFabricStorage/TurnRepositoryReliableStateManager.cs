using System;
using IUGO.Shipping.Core;
using IUGO.Shipping.Infrastructure.Data.ServiceFabricStorage.DTOs;
using IUGO.Storage.AzureServiceFabric;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace IUGO.Shipping.Infrastructure.Data.ServiceFabricStorage
{
    using Core = Core.ShippingAggregate;

    public class ShippingRepositoryReliableStateManager : AggregateGenericRepositoryReliableStateManager<Core.Shipping,DTOs.Shipping>,IShippingRepository
    {
        public ShippingRepositoryReliableStateManager(IReliableDictionary<Guid, DTOs.Shipping> aggreateStorage,
            ITransaction transaction) : base(aggreateStorage, transaction, new ShippingMapper())
        {
        }
    }
}

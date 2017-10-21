using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IUGO.Storage.AzureServiceFabric;
using IUGO.Turns.Core;
using IUGO.Turns.Infrastructure.Data.ServiceFabricStorage.DTOs;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Turn = IUGO.Turns.Core.TurnAggreate.Turn;

namespace IUGO.Turns.Infrastructure.Data.ServiceFabricStorage
{
    using Core = Core.TurnAggreate;
    public class TurnRepositoryReliableStateManager : AggregateGenericRepositoryReliableStateManager<Core.Turn, DTOs.Turn> , ITurnRepository
    {
        public TurnRepositoryReliableStateManager(IReliableDictionary<Guid, DTOs.Turn> aggreateStorage,
            ITransaction transaction)
            : base(aggreateStorage, transaction, new TurnMapper())
        {
        }
    }
}

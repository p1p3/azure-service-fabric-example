using System;
using System.Threading.Tasks;
using IUGO.Turns.Core;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace IUGO.Turns.Infrastructure.Data.ServiceFabricStorage
{
    public class UnitOfWorkReliableStateManager : IUnitOfWork
    {
        private readonly IReliableStateManager _stateManager;
        private ITransaction _transaction;
        
        public UnitOfWorkReliableStateManager(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
            _transaction = stateManager.CreateTransaction();
        }

        public Task<ITurnRepository> TurnsRepository => this.CreateTurnsRepository();

        public async Task Commit()
        {
            await this._transaction.CommitAsync();
            this._transaction.Dispose();        
        }

        private async Task<ITurnRepository> CreateTurnsRepository()
        {
            var turnsStorage = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, DTOs.Turn>>("turns");
            return new TurnRepositoryReliableStateManager(turnsStorage, _transaction);
        }

        public void Dispose()
        {
            this._transaction?.Dispose();
        }
    }
}

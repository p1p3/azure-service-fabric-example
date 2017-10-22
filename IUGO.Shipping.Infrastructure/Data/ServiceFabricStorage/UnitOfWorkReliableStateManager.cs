using System;
using System.Threading.Tasks;
using IUGO.Shippings.Core;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace IUGO.Shippings.Infrastructure.Data.ServiceFabricStorage
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

        public Task<IShippingRepository> ShippingsRepository => this.CreateShippingsRepository();

        public async Task Commit()
        {
            await this._transaction.CommitAsync();
            this._transaction.Dispose();
            
            _transaction = _stateManager.CreateTransaction();
        }

        private async Task<IShippingRepository> CreateShippingsRepository()
        {
            var shippingStorage = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, DTOs.Shipping>>("shippings");
            return new ShippingRepositoryReliableStateManager(shippingStorage, _transaction);
        }

        public void Dispose()
        {
            this._transaction?.Dispose();
        }
    }
}

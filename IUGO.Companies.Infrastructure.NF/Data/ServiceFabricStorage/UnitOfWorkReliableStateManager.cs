using System;
using System.Threading.Tasks;
using IUGO.Companies.Core;
using IUGO.Companies.Core.Repositories;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace IUGO.Companies.Infrastructure.Data.ServiceFabricStorage
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

        public Task<ICompanyRepository> CompanyRepository => this.CreateCompanyRepository();

        public async Task Commit()
        {
            await this._transaction.CommitAsync();
            this._transaction.Dispose();


            _transaction = _stateManager.CreateTransaction();
        }

        private async Task<ICompanyRepository> CreateCompanyRepository()
        {
            var companyStorage = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Company>>("companies");
            return new CompanyRepositoryReliableStateManager(companyStorage, _transaction);
        }

        public void Dispose()
        {
            this._transaction?.Dispose();
        }
    }
}

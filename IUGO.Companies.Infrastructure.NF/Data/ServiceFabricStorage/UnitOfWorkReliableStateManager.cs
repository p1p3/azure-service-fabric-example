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
        private readonly ITransaction _transaction;
        private Task<ICompanyRepository> _companyRepository;

        public UnitOfWorkReliableStateManager(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
            _transaction = stateManager.CreateTransaction();
        }

        public Task<ICompanyRepository> CompanyRepository => this._companyRepository ?? (this._companyRepository = this.CreateCompanyRepository());

        public Task Commit()
        {
            try
            {
                return this._transaction.CommitAsync();
            }
            catch (Exception)
            {
                this._transaction.Abort();
                throw;
            }
           
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

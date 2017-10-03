using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IUGO.Companies.Core;
using IUGO.Companies.Core.Repositories;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;

namespace IUGO.Companies.Infrastructure.Data.ServiceFabricStorage
{
    public class CompanyRepositoryReliableStateManager : ICompanyRepository
    {
        private readonly IReliableDictionary<Guid, Company> _companyStorage;
        private readonly ITransaction _transaction;

        public CompanyRepositoryReliableStateManager(IReliableDictionary<Guid, Company> companyStorage,
            ITransaction transaction)
        {
            this._companyStorage = companyStorage;
            this._transaction = transaction;
        }


        public Task<Company> Create(Company company)
        {
            return _companyStorage.AddOrUpdateAsync(this._transaction, company.Id, company, (id, value) => company);
        }

        public async Task<IEnumerable<Company>> List()
        {
            var result = new List<Company>();

            var allCompanies = await _companyStorage.CreateEnumerableAsync(_transaction, EnumerationMode.Unordered);
            using (var enumerator = allCompanies.GetAsyncEnumerator())
            {
                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    KeyValuePair<Guid, Company> current = enumerator.Current;
                    result.Add(current.Value);
                }
            }

            return result;
        }
    }
}

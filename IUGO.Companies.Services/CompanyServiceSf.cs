using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGO.Companies.Core;
using IUGO.Companies.Infrastructure.Data;
using IUGO.Companies.Infrastructure.Data.ServiceFabricStorage;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace IUGO.Companies.Services
{
    internal class CompanyServiceSf : StatefulService, ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private CancellationToken _cancellationToken;

        /// <inheritdoc />
        protected override Task RunAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(this.CreateServiceRemotingListener)
            };
        }

        public CompanyServiceSf(StatefulServiceContext serviceContext) : base(serviceContext)
        {
            _unitOfWork =  new UnitOfWorkReliableStateManager(this.StateManager); ;
        }

 
        public async Task AddCompany(DTOs.Company company)
        {
            var domainCompany = new Company() { Id = company.Id, Name = company.Name };
            var repo = await _unitOfWork.CompanyRepository;
            var newCompany = await repo.Create(domainCompany);
        }

        public async Task<IEnumerable<DTOs.Company>> ListAll()
        {
            var repo = await _unitOfWork.CompanyRepository;
            var companies = await repo.List();
            return companies.Select(company => new DTOs.Company() {Id = company.Id, Name = company.Name});
        }
    }
}
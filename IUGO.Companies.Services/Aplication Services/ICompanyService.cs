using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IUGO.Companies.Core;
using IUGO.Companies.Core.Repositories;
using IUGO.Companies.Infrastructure.Data;
using IUGO.Companies.Infrastructure.Data.ServiceFabricStorage;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace IUGO.Companies.Services.Aplication_Services
{
    public interface ICompanyService : IService
    {
        Task AddCompany(DTOs.Company company);
    }

    public class CompanyServiceSf : StatefulService, ICompanyService
    {
        private IUnitOfWork _unitOfWork;
        private CancellationToken _cancellationToken;

        /// <inheritdoc />
        protected override Task RunAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager);
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
            //_unitOfWork = unitOfWork;
        }

        //public CompanyServiceSf(StatefulServiceContext serviceContext, IUnitOfWork unitOfWork) : base(serviceContext)
        //{
        //    _unitOfWork = unitOfWork;
        //}

        public async Task AddCompany(DTOs.Company company)
        {
            var domainCompany = new Company() { Id = company.Id, Name = company.Name };
            var repo = await _unitOfWork.CompanyRepository;
            var newCompany = await repo.Create(domainCompany);
        }
    }
}

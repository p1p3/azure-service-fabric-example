using System;
using System.Threading.Tasks;
using IUGO.Companies.Core.Repositories;

namespace IUGO.Companies.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<ICompanyRepository> CompanyRepository { get; }
        Task Commit();
    }
}

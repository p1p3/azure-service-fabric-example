using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace IUGO.Companies.Services
{
    public interface ICompanyService : IService
    {
        Task AddCompany(DTOs.Company company);
        Task<IEnumerable<DTOs.Company>> ListAll();
    }
}

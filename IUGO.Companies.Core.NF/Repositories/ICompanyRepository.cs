using System.Collections.Generic;
using System.Threading.Tasks;

namespace IUGO.Companies.Core.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> Create(Company company);
        Task<IEnumerable<Company>> List();
    }
}

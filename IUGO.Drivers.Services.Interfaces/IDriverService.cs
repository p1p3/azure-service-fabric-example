using System.Threading.Tasks;
using IUGO.Drivers.Services.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Remoting;

namespace IUGO.Drivers.Services.Interfaces
{
    public interface IDriverService : IService
    {
        Task<Driver> CreateDriver(Driver driver);
    }
}
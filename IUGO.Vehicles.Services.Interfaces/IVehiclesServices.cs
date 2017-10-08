using System.Collections.Generic;
using System.Threading.Tasks;
using IUGO.Vehicles.Services.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Remoting;

namespace IUGO.Vehicles.Services.Interfaces
{
    public interface IVehiclesServices : IService
    {
        Task<Vehicle> CreateVehicle(Vehicle veicle);

        Task<Vehicle> FindVehicle(string plateNumber);

        Task<IEnumerable<Vehicle>> List();
    }
}

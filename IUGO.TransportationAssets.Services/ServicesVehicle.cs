using System;
using System.Threading.Tasks;
using IUGO.TransportationAssets.Services.Interfaces;

namespace IUGO.TransportationAssets.Services
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed partial class Services : IVehiclesServices
    {

        Task IVehiclesServices.DoSomthing()
        {
            Console.WriteLine("holi");
            return Task.CompletedTask;
        }


    }
}

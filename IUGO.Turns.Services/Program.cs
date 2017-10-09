using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using IUGO.Vehicles.Services.Interfaces;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Runtime;

namespace IUGO.Turns.Services
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.

                ServiceRuntime.RegisterServiceAsync("IUGO.Turns.ServicesType",
                    context => new TurnServices(context, CreateVehicleService())).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(TurnServices).Name);

                // Prevents this host process from terminating so services keep running.
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }


        private static IVehiclesServices CreateVehicleService()
        {
            var uri = "fabric:/IUGOsf/IUGO.Vehicles.Services";

            return ServiceProxy.Create<IVehiclesServices>(
                new Uri(uri),
                new ServicePartitionKey(0));
        }
    }


}

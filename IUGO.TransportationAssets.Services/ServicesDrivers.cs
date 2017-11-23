using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGO.TransportationAssets.Services.Interfaces;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace IUGO.TransportationAssets.Services
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed partial class Services : IDriverService
    {

        Task IDriverService.DoSomthing()
        {
            Console.WriteLine("holi");
            return Task.CompletedTask;
        }

    }
}

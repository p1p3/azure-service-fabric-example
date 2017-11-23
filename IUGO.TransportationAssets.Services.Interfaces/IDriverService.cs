using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace IUGO.TransportationAssets.Services.Interfaces
{
    public interface IDriverService : IService
    {
        Task DoSomthing();
    }
}

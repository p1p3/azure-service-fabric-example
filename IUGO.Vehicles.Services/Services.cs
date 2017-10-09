using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using IUGO.Vehicles.Services.Interfaces;
using IUGO.Vehicles.Services.Interfaces.Models;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace IUGO.Vehicles.Services
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Services : StatefulService, IVehiclesServices
    {
        private IReliableDictionary<string, Vehicle> _vehicles;

        public Services(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(this.CreateServiceRemotingListener)
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            this._vehicles = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, Vehicle>>("vehicles");
        }

        public async Task<Vehicle> CreateVehicle(Vehicle vehicle)
        {
            using (var tx = this.StateManager.CreateTransaction())
            {
                var createdVehicle = await _vehicles.AddOrUpdateAsync(tx, vehicle.PlateNumber, vehicle, (key, value) => vehicle);
                await tx.CommitAsync();
                return createdVehicle;
            }
        }

        public async Task<Vehicle> FindVehicle(string plateNumber)
        {
            using (var tx = this.StateManager.CreateTransaction())
            {
                var vehicle = await _vehicles.TryGetValueAsync(tx, plateNumber);
                return vehicle.Value;
            }
        }

        public async Task<IEnumerable<Vehicle>> List()
        {
            var result = new List<Vehicle>();

            using (var tx = this.StateManager.CreateTransaction())
            {
                var allVehicles = await _vehicles.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allVehicles.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<string, Vehicle> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }
    }
}

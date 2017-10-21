using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUGO.Storage.AzureServiceFabric;

namespace IUGO.Turns.Infrastructure.Data.ServiceFabricStorage.DTOs
{
    public class Turn : IStorableItem
    {
        public Guid Id { get; set; }
        public DateTime QueuedFrom { get; set; }
        public DateTime AvailableFrom { get; set; }
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public IEnumerable<string> OriginIds { get; set; }
        public IEnumerable<string> DestiniationIds { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUGO.Turns.Services.Interface.Integration
{
    public class TurnAssignedMessage
    {
        public string Id { get; set; }
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
        public string ShippingServiceId { get; set; }
    }
}

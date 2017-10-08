using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUGO.Turns.Services.Interface.Models
{
    public class TurnInputModel
    {
        public DateTime AvailableFrom { get; set; }
        public string DriverId { get; set; }
        public string VehicleId { get; set; }
    }
}

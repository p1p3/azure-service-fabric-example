using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IUGO.Turns.API.Models
{
    public class TurnsFilter
    {
        public string DestinationId { get; set; }
        public string OriginId { get; set; }
        public DateTime PickUpDate { get; set; }
    }
}

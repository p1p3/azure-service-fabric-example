using System;
using System.Collections.Generic;

namespace IUGO.Shippings.Services.Interfaces.Models
{
    public class ShippingInputModel
    {
        public string OrignId { get; set; }
        public string DestinationId { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime AllocationDeadline { get; set; }
        public string ShippingServiceName { get; set; }
        public double ShippingCost { get; set; }
        public string Comments { get; set; }
    }
}
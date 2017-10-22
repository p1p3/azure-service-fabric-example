using System;
using System.Collections.Generic;
using IUGO.Storage.AzureServiceFabric;

namespace IUGO.Shippings.Infrastructure.Data.ServiceFabricStorage.DTOs
{
    public class Shipping : IStorableItem
    {
        public Guid Id { get; set; }
        public string OrignId { get; set; }
        public string DestinationId { get; set; }
        public DateTime CeationDate { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime AllocationDeadline { get; set; }
        public string ShippingServiceName { get; set; }
        public double ShippingCost { get; set; }
        public string Comments { get; set; }
        public IEnumerable<string> RequiredVehicleDesignationsIds { get; set; }
        public DateTime FinalPickUpDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public ShippingTurn AssignedTurn { get; set; }
        public int ShippingState { get; set; }

    }
}

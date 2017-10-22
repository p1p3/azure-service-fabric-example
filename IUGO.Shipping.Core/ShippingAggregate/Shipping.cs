using System;
using System.Collections.Generic;
using IUGO.Domain;

namespace IUGO.Shippings.Core.ShippingAggregate
{
    public class Shipping : IAggregate
    {
        public Shipping(Guid id, string orignId, string destinationId, DateTime pickUpDate, string shippingServiceName, double shippingCost,
            DateTime allocationDeadline, string comments, DateTime creationDate, List<string> requiredVehicleDesginationIds)
        {
            Id = id;
            OrignId = orignId;
            DestinationId = destinationId;
            PickUpDate = pickUpDate;
            ShippingServiceName = shippingServiceName;
            ShippingCost = shippingCost;
            AllocationDeadline = allocationDeadline;
            Comments = comments;
            CeationDate = creationDate;
            _requiredVehicleDesignationsIds = requiredVehicleDesginationIds;
        }

        public Shipping(string orignId, string destinationId, DateTime pickUpDate, string shippingServiceName, double shippingCost,
            DateTime allocationDeadline, string comments)
            : this(Guid.NewGuid(), orignId, destinationId, pickUpDate, shippingServiceName, shippingCost, allocationDeadline, comments,DateTime.Now,new List<string>())
        {
        }



        public Guid Id { get; }
        public string OrignId { get; }
        public string DestinationId { get; }
        public DateTime CeationDate { get; }
        public DateTime PickUpDate { get; }
        public DateTime AllocationDeadline { get; }
        public string ShippingServiceName { get; }
        public double ShippingCost { get; }
        public string Comments { get; }

        public IReadOnlyCollection<string> RequiredVehicleDesignationsIds => _requiredVehicleDesignationsIds;
        private readonly List<string> _requiredVehicleDesignationsIds;

        public void AddRequiredVehicleDesignation(string id)
        {
            this._requiredVehicleDesignationsIds.Add(id);
        }



    }
}

using System;
using System.Collections.Generic;
using IUGO.Domain;

namespace IUGO.Shippings.Core.ShippingAggregate
{
    public class Shipping : IAggregate
    {
        public Shipping(Guid id, string orignId, string destinationId, DateTime pickUpDate, string shippingServiceName, double shippingCost,
            DateTime allocationDeadline, string comments, DateTime creationDate, List<string> requiredVehicleDesginationIds, ShippingStates state,
            ShippingTurn assignedturn, DateTime finalPickUpDate, DateTime deliveryDate)
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
            AssignedTurn = assignedturn;
            FinalPickUpDate = finalPickUpDate;
            DeliveryDate = deliveryDate;
        }

        public static Shipping CreateShipping(string orignId, string destinationId, DateTime pickUpDate,
                  string shippingServiceName, double shippingCost,
                  DateTime allocationDeadline, string comments)
        {
            return new Shipping(Guid.NewGuid(), orignId,
                destinationId, pickUpDate,
                shippingServiceName, shippingCost,
                allocationDeadline, comments,
                DateTime.Now, new List<string>(),
                ShippingStates.Created,
                new NullShippingTurn(),
                default(DateTime),
                default(DateTime));
        }


        public Guid Id { get; }
        public string OrignId { get; }
        public string DestinationId { get; }
        public DateTime CeationDate { get; }
        public DateTime PickUpDate { get; }
        public DateTime FinalPickUpDate { get; private set; }
        public DateTime AllocationDeadline { get; }
        public string ShippingServiceName { get; }
        public double ShippingCost { get; }
        public string Comments { get; }
        public ShippingStates ShippingState { get; private set; }
        public DateTime DeliveryDate { get; private set; }

        public ShippingTurn AssignedTurn { get; private set; }


        public IReadOnlyCollection<string> RequiredVehicleDesignationsIds => _requiredVehicleDesignationsIds;
        private readonly List<string> _requiredVehicleDesignationsIds;

        public void AddRequiredVehicleDesignation(string id)
        {
            this._requiredVehicleDesignationsIds.Add(id);
        }

        public void AssignTurn(ShippingTurn turn)
        {
            this.AssignedTurn = turn;
            this.ShippingState = ShippingStates.Assigned;
        }


        public void ChangeStateToDelivered()
        {
            this.DeliveryDate = DateTime.Now; ;
            this.ShippingState = ShippingStates.Delivired;
        }

        public void ChangeStateToPickedUp()
        {
            this.FinalPickUpDate = DateTime.Now;
            this.ShippingState = ShippingStates.PickedUp;
        }



    }
}

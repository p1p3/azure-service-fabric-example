using System;
using System.Collections.Generic;
using IUGO.Domain;

namespace IUGO.Turns.Core.TurnAggreate
{
    public class Turn : IAggregate
    {

        public Turn(Guid id, DateTime queuedFrom, DateTime availableFrom, string driverId, string vehicleId,
            List<string> originIds, List<string> destiniationIds, string vehicleDesignationId, List<string> acceptedShippingsOffers, string shippingId)
        {
            Id = id;
            QueuedFrom = queuedFrom;
            AvailableFrom = availableFrom;
            DriverId = driverId;
            VehicleId = vehicleId;
            _originIds = originIds;
            _destiniationIds = destiniationIds;
            VehicleDesignationId = vehicleDesignationId;
            _acceptedShippingsOffers = acceptedShippingsOffers;
            ShippingId = shippingId;
        }

        public Turn(DateTime availableFrom, string driverId, string vehicleId, List<string> originIds, List<string> destiniationIds, string vehicleDesignationId, List<string> acceptedShippingsOffers, string shippingId) :
            this(Guid.NewGuid(), DateTime.Now, availableFrom, driverId, vehicleId, originIds, destiniationIds, vehicleDesignationId, acceptedShippingsOffers, shippingId)
        {


        }
        public Turn(DateTime availableFrom, string driverId, string vehicleId, string vehicleDesignationId)
            : this(availableFrom, driverId, vehicleId, new List<string>(), new List<string>(), vehicleDesignationId, new List<string>(), "")
        {

        }

        public Guid Id { get; private set; }
        public DateTime QueuedFrom { get; private set; }
        public DateTime AvailableFrom { get; private set; }

        public bool IsTurnAssigned => !string.IsNullOrEmpty(ShippingId);

        public void AddOrigin(string originId)
        {
            if (!this._originIds.Contains(originId))
                this._originIds.Add(originId);
        }
        public void AddDestiniation(string destinationId)
        {
            if (!this._destiniationIds.Contains(destinationId))
                this._destiniationIds.Add(destinationId);
        }

        public void AssignShipping(string shippingId)
        {
            if (_acceptedShippingsOffers.Contains(shippingId))
            {
                this.ShippingId = shippingId;
            }
            else
            {
                throw new Exception("Shipping has not been accepted in the turn");
            }
        }
        public void AcceptShippingOffer(string shippingId)
        {
            if (!this._acceptedShippingsOffers.Contains(shippingId))
                this._acceptedShippingsOffers.Add(shippingId);
        }


        //TODO: Info about the driver.
        //TODO: Info about the vehicle

        #region External Details

        public string DriverId { get; private set; }
        public string VehicleId { get; private set; }
        public string VehicleDesignationId { get; private set; }

        public IReadOnlyCollection<string> OriginIds => _originIds;
        private readonly List<string> _originIds;

        public IReadOnlyCollection<string> DestiniationIds => _destiniationIds;
        private readonly List<string> _destiniationIds;

        public string ShippingId { get; private set; }
        public IReadOnlyCollection<string> AcceptedShippingsOffers => _acceptedShippingsOffers;
        private readonly List<string> _acceptedShippingsOffers;



        #endregion

    }
}

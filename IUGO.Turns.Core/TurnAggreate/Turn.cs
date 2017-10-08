using System;
using System.Collections.Generic;

namespace IUGO.Turns.Core.TurnAggreate
{
    public class Turn
    {

        public Turn(Guid id, DateTime queuedFrom, DateTime availableFrom, string driverId, string vehicleId,
            List<string> originIds, List<string> destiniationIds)
        {
            Id = id;
            QueuedFrom = queuedFrom;
            AvailableFrom = availableFrom;
            DriverId = driverId;
            VehicleId = vehicleId;
            _originIds = originIds;
            _destiniationIds = destiniationIds;
        }

        public Turn(DateTime availableFrom, string driverId, string vehicleId, List<string> originIds, List<string> destiniationIds) :
            this(Guid.NewGuid(), DateTime.Now, availableFrom, driverId, vehicleId, originIds, destiniationIds)
        {


        }
        public Turn(DateTime availableFrom, string driverId, string vehicleId)
            : this(availableFrom, driverId, vehicleId, new List<string>(), new List<string>())
        {

        }

        public Guid Id { get; private set; }
        public DateTime QueuedFrom { get; private set; }
        public DateTime AvailableFrom { get; private set; }

        public void AddOrigin(string originId)
        {
            this._originIds.Add(originId);
        }
        public void AddDestiniation(string destinationId)
        {
            this._destiniationIds.Add(destinationId);
        }

        #region External Details

        public string DriverId { get; private set; }
        public string VehicleId { get; private set; }

        public IReadOnlyCollection<string> OriginIds => _originIds;
        private readonly List<string> _originIds;

        public IReadOnlyCollection<string> DestiniationIds => _destiniationIds;
        private readonly List<string> _destiniationIds;

        #endregion

    }
}

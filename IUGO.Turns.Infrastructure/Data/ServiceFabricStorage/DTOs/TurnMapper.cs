using System.Linq;
using IUGO.Storage.AzureServiceFabric;

namespace IUGO.Turns.Infrastructure.Data.ServiceFabricStorage.DTOs
{
    using Core = Core.TurnAggreate;

    public class TurnMapper : IStorableEntityMapper<Core.Turn, DTOs.Turn>
    {
       
        public Turn MapToStorable(Core.Turn entity)
        {
            return new DTOs.Turn()
            {
                OriginIds = entity.OriginIds,
                DestiniationIds = entity.DestiniationIds,
                AvailableFrom = entity.AvailableFrom,
                DriverId = entity.DriverId,
                Id = entity.Id,
                QueuedFrom = entity.QueuedFrom,
                VehicleId = entity.VehicleId
            };
        }

        public Core.Turn MapToCore(Turn entity)
        {
            return new Core.Turn(entity.Id, entity.QueuedFrom
                , entity.AvailableFrom
                , entity.DriverId
                , entity.VehicleId
                , entity.OriginIds.ToList()
                , entity.DestiniationIds.ToList());
        }
    }
}

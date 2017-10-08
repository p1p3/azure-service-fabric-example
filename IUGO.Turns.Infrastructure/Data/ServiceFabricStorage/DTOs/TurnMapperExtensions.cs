using System.Linq;


namespace IUGO.Turns.Infrastructure.Data.ServiceFabricStorage.DTOs
{
    using Core = Core.TurnAggreate;

    public static class TurnMapperExtensions
    {
        public static DTOs.Turn MapToRepository(this Core.Turn turn)
        {
            return new DTOs.Turn()
            {
                OriginIds = turn.OriginIds,
                DestiniationIds = turn.DestiniationIds,
                AvailableFrom = turn.AvailableFrom,
                DriverId = turn.DriverId,
                Id = turn.Id,
                QueuedFrom = turn.QueuedFrom,
                VehicleId = turn.VehicleId
            };
        }

        public static Core.Turn MapToCore(this DTOs.Turn turn)
        {
            return new Core.Turn(turn.Id,turn.QueuedFrom
                ,turn.AvailableFrom
                ,turn.DriverId
                ,turn.VehicleId
                ,turn.OriginIds.ToList()
                ,turn.DestiniationIds.ToList());
        }
    }
}

using System.Linq;

namespace IUGO.Turns.Services.Mappers
{
    using Core = Core.TurnAggreate;
    using Interface = Interface.Models;

    public static class MapperExtensions
    {
        public static Interface.OutputTurnModel MapToOutputTurnModel(this Core.Turn turn)
        {
            return new Interface.OutputTurnModel()
            {
                OriginIds = turn.OriginIds,
                DestiniationIds = turn.DestiniationIds,
                AvailableFrom = turn.AvailableFrom,
                DriverId = turn.DriverId,
                Id = turn.Id.ToString(),
                QueuedFrom = turn.QueuedFrom,
                VehicleId = turn.VehicleId,
                VehicleDesignationId = turn.VehicleDesignationId
            };
        }

 
    }
}

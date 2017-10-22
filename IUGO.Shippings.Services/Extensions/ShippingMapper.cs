using System.Linq;

namespace IUGO.Shippings.Services.Extensions
{
    using Core = Core.ShippingAggregate;
    using Interfaces = Interfaces.Models;

    public static class ShippingMapper
    {
        #region Interfaces 
        public static Interfaces.ShippingOutputModel MapToInterfaces(this Core.Shipping entity)
        {
            return new Interfaces.ShippingOutputModel()
            {
                ShippingState = (int) entity.ShippingState,
                Id = entity.Id,
                FinalPickUpDate = entity.FinalPickUpDate,
                CeationDate = entity.CeationDate,
                DeliveryDate = entity.DeliveryDate,
                AssignedTurn = entity.AssignedTurn.MapToInterfaces(),
                OrignId = entity.OrignId,
                PickUpDate = entity.FinalPickUpDate,
                AllocationDeadline = entity.AllocationDeadline,
                Comments = entity.Comments,
                ShippingCost = entity.ShippingCost,
                DestinationId = entity.DestinationId,
                ShippingServiceName = entity.ShippingServiceName,
                RequiredVehicleDesignationsIds = entity.RequiredVehicleDesignationsIds,
                Candidates = entity.Candidates.Select(MapToInterfaces)
            };
        }

        public static Interfaces.ShippingVehicle MapToInterfaces(this Core.ShippingVehicle entity)
        {
            return new Interfaces.ShippingVehicle()
            {
                Id = entity.Id,
                DesignationId = entity.DesignationId
            };
        }

        public static Interfaces.ShippingDriver MapToInterfaces(this Core.ShippingDriver entity)
        {
            return new Interfaces.ShippingDriver()
            {
                Id = entity.Id,
                ContactNumber = entity.ContactNumber,
                FullName = entity.FullName
            };
        }

        public static Interfaces.ShippingTurn MapToInterfaces(this Core.ShippingTurn entity)
        {
            return new Interfaces.ShippingTurn()
            {
                Vehicle = entity.Vehicle.MapToInterfaces(),
                Driver = entity.Driver.MapToInterfaces(),
                TurnId = entity.TurnId
            };
        }
        #endregion

        #region Core

        public static Core.ShippingDriver MapToCore(this Interfaces.ShippingDriver entity)
        {
            return Core.ShippingDriver.CreateShippingDriver(entity.Id,entity.ContactNumber,entity.FullName);
        }

        public static Core.ShippingVehicle MapToCore(this Interfaces.ShippingVehicle entity)
        {
            return Core.ShippingVehicle.CreateShippingVehicle(entity.DesignationId, entity.Id);
        }

        public static Core.ShippingTurn MapToCore(this Interfaces.ShippingTurn entity)
        {
            return Core.ShippingTurn.CreateShippingTurn(entity.TurnId,entity.Driver.MapToCore(),entity.Vehicle.MapToCore());
        }

        #endregion
    }
}
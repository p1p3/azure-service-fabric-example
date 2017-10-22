using System;
using System.Linq;
using IUGO.Shippings.Core.ShippingAggregate;
using IUGO.Storage.AzureServiceFabric;

namespace IUGO.Shippings.Infrastructure.Data.ServiceFabricStorage.DTOs
{
    public class ShippingMapper : IStorableEntityMapper<Core.ShippingAggregate.Shipping, DTOs.Shipping>
    {

        #region ToCoreMappers

        public Core.ShippingAggregate.Shipping MapToCore(Shipping entity)
        {


            return new Core.ShippingAggregate.Shipping(entity.Id, entity.OrignId, entity.DestinationId, entity.PickUpDate,
                entity.ShippingServiceName,
                entity.ShippingCost,
                entity.AllocationDeadline,
                entity.Comments,
                entity.CeationDate,
                entity.RequiredVehicleDesignationsIds.ToList(),
                (ShippingStates)entity.ShippingState,
                MapToCore(entity.AssignedTurn),
                entity.FinalPickUpDate,
                entity.DeliveryDate,
                entity.Candidates.Select(MapToCore).ToList());
        }

        private Core.ShippingAggregate.ShippingDriver MapToCore(ShippingDriver entity)
        {
            return Core.ShippingAggregate.ShippingDriver.CreateShippingDriver(entity.Id, entity.ContactNumber, entity.FullName);
        }

        private Core.ShippingAggregate.ShippingVehicle MapToCore(ShippingVehicle entity)
        {
            return Core.ShippingAggregate.ShippingVehicle.CreateShippingVehicle(entity.DesignationId, entity.Id);
        }

        private Core.ShippingAggregate.ShippingTurn MapToCore(ShippingTurn entity)
        {
            return Core.ShippingAggregate.ShippingTurn.CreateShippingTurn(entity.TurnId, MapToCore(entity.Driver), MapToCore(entity.Vehicle));
        }

        #endregion

        #region ToStorable

        public Shipping MapToStorable(Core.ShippingAggregate.Shipping entity)
        {
            return new DTOs.Shipping()
            {
                Id = entity.Id,
                CeationDate = entity.CeationDate,
                AllocationDeadline = entity.AllocationDeadline,
                Comments = entity.Comments,
                DestinationId = entity.DestinationId,
                OrignId = entity.OrignId,
                PickUpDate = entity.PickUpDate,
                RequiredVehicleDesignationsIds = entity.RequiredVehicleDesignationsIds,
                ShippingCost = entity.ShippingCost,
                ShippingServiceName = entity.ShippingServiceName,
                ShippingState = (int)entity.ShippingState,
                AssignedTurn = MapToStorable(entity.AssignedTurn),
                FinalPickUpDate = entity.FinalPickUpDate,
                DeliveryDate = entity.DeliveryDate,
                Candidates =  entity.Candidates.Select(MapToStorable)
            };
        }

        private ShippingDriver MapToStorable(Core.ShippingAggregate.ShippingDriver entity)
        {
            return new ShippingDriver
            {
                Id = entity.Id,
                ContactNumber = entity.ContactNumber,
                FullName =  entity.FullName
            };
        }

        private ShippingVehicle MapToStorable(Core.ShippingAggregate.ShippingVehicle entity)
        {
            return new ShippingVehicle { Id =  entity.Id, DesignationId = entity.DesignationId};
        }


        private ShippingTurn MapToStorable(Core.ShippingAggregate.ShippingTurn entity)
        {
            return new ShippingTurn
            {
                Vehicle = MapToStorable(entity.Vehicle),
                Driver = MapToStorable(entity.Driver),
                TurnId = entity.TurnId
            };
        }

        #endregion


    }

}

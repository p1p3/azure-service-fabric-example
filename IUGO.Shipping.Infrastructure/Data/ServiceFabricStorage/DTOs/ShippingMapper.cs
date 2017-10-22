using System.Linq;
using IUGO.Storage.AzureServiceFabric;

namespace IUGO.Shippings.Infrastructure.Data.ServiceFabricStorage.DTOs
{
    public  class ShippingMapper : IStorableEntityMapper<Core.ShippingAggregate.Shipping,DTOs.Shipping>
    {
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
                ShippingServiceName = entity.ShippingServiceName
            };
        }

        public Core.ShippingAggregate.Shipping MapToCore(Shipping entity)
        {
            return new Core.ShippingAggregate.Shipping(entity.Id, entity.OrignId, entity.DestinationId, entity.PickUpDate,
                entity.ShippingServiceName,
                entity.ShippingCost,
                entity.AllocationDeadline,
                entity.Comments,
                entity.CeationDate,
                entity.RequiredVehicleDesignationsIds.ToList());
        }
    }
}

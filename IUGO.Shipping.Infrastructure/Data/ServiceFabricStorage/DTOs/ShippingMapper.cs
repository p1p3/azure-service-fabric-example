using System.Linq;
using IUGO.Storage.AzureServiceFabric;

namespace IUGO.Shipping.Infrastructure.Data.ServiceFabricStorage.DTOs
{
    using Core = Core.ShippingAggregate;

    public  class ShippingMapper : IStorableEntityMapper<Core.Shipping,DTOs.Shipping>
    {
        public Shipping MapToStorable(Core.Shipping entity)
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

        public Core.Shipping MapToCore(Shipping entity)
        {
            return new Core.Shipping(entity.Id, entity.OrignId, entity.DestinationId, entity.PickUpDate,
                entity.ShippingServiceName,
                entity.ShippingCost,
                entity.AllocationDeadline,
                entity.Comments,
                entity.CeationDate,
                entity.RequiredVehicleDesignationsIds.ToList());
        }
    }
}

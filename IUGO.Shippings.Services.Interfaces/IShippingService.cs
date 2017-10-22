using System;
using System.Threading.Tasks;
using IUGO.Shippings.Services.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Remoting;

namespace IUGO.Shippings.Services.Interfaces
{
    public interface IShippingService : IService
    {
        Task<ShippingOutputModel> FindShipping(Guid id);
        Task<ShippingOutputModel> CreateShipping(ShippingInputModel shippingInputModel);
        Task<ShippingOutputModel> AddRequiredVehicleDesignation(Guid id, string vehicleDesignationId);
        Task<ShippingOutputModel> AddCandidate(ShippingTurn canadidate, Guid shippingId);
        Task<ShippingOutputModel> AssignTurn(ShippingTurn turn, Guid shippingId);
        Task SetShippingAsDelivered(Guid id);
        Task SetShippingAsPickedUp(Guid id);
        Task PublishShipping(Guid id);

        //TODO: StartSearchForCompliantTurns
        //TODO: FindShippingsForTurns --> Notify
    }
}

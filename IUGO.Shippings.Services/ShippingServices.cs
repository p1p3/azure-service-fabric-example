using System;
using System.Collections.Generic;
using System.Fabric;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using IUGO.Shippings.Core.ShippingAggregate;
using IUGO.Shippings.Infrastructure.Data;
using IUGO.Shippings.Infrastructure.Data.ServiceFabricStorage;
using IUGO.Shippings.Infrastructure.Integration;
using IUGO.Shippings.Services.Extensions;
using IUGO.Shippings.Services.Interfaces;
using IUGO.Shippings.Services.Interfaces.Integration;
using IUGO.Shippings.Services.Interfaces.Models;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using ShippingTurn = IUGO.Shippings.Services.Interfaces.Models.ShippingTurn;

namespace IUGO.Shippings.Services
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ShippingServices : StatefulService, IShippingService
    {
        private readonly EventEmitter<ShippingPublishedIntegrationEvent> _shippingPublishedEventEmitter;
        private readonly EventEmitter<ShippingOfferAcceptedIntegrationEvent> _shippingOfferAccepted;

        //private IUnitOfWork unitOfWork;
        private CancellationToken _cancellationToken;

        public ShippingServices(StatefulServiceContext context, EventEmitter<ShippingPublishedIntegrationEvent> shippingPublishedEventEmitter, EventEmitter<ShippingOfferAcceptedIntegrationEvent> shippingOfferAccepted)
            : base(context)
        {
            _shippingPublishedEventEmitter = shippingPublishedEventEmitter;
            _shippingOfferAccepted = shippingOfferAccepted;
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(this.CreateServiceRemotingListener)
            };
        }

        protected override Task RunAsync(CancellationToken cancellationToken)
        {
           // unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager);
            _cancellationToken = cancellationToken;
            return Task.CompletedTask;
        }

        public async Task<ShippingOutputModel> FindShipping(Guid id)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.ShippingsRepository;
                var shipping = await repo.Find(id);

                return shipping.MapToInterfaces();
            }
        }

        public async Task<ShippingOutputModel> CreateShipping(ShippingInputModel shippingInputModel)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var shipping = Shipping.CreateShipping(shippingInputModel.OrignId,
                    shippingInputModel.DestinationId,
                    shippingInputModel.PickUpDate,
                    shippingInputModel.ShippingServiceName,
                    shippingInputModel.ShippingCost,
                    shippingInputModel.AllocationDeadline, shippingInputModel.Comments);

                var repo = await unitOfWork.ShippingsRepository;

                var createdShipping = await repo.Add(shipping);
                await unitOfWork.Commit();

                return createdShipping.MapToInterfaces();
            }
        }

        public async Task<ShippingOutputModel> AddRequiredVehicleDesignation(Guid id, string vehicleDesignationId)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.ShippingsRepository;
                var shipping = await repo.Find(id);
                shipping.AddRequiredVehicleDesignation(vehicleDesignationId);

                await UpdateShipping(shipping, unitOfWork);

                return shipping.MapToInterfaces();
            }
        }

        public async Task<ShippingOutputModel> AddCandidate(ShippingTurn canadidate, Guid shippingId)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.ShippingsRepository;
                var shipping = await repo.Find(shippingId);
                shipping.AddCandidate(canadidate.MapToCore());

                await UpdateShipping(shipping, unitOfWork);

                var eventMessage = new ShippingOfferAcceptedIntegrationEvent()
                {
                    ShippingId = shippingId.ToString(),
                    TurnId = canadidate.TurnId
                };

                _shippingOfferAccepted.Emit(eventMessage);

                return shipping.MapToInterfaces();
            }
        }

        public async Task<ShippingOutputModel> AssignTurn(ShippingTurn turn, Guid shippingId)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.ShippingsRepository;
                var shipping = await repo.Find(shippingId);

                shipping.AssignTurn(turn.MapToCore());

                await UpdateShipping(shipping, unitOfWork);

                return shipping.MapToInterfaces();
                //TODO emit event to notify driver 
            }
        }


        public async Task SetShippingAsDelivered(Guid id)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.ShippingsRepository;
                var shipping = await repo.Find(id);

                shipping.ChangeStateToDelivered();
                await UpdateShipping(shipping, unitOfWork);
            }
        }

        public async Task SetShippingAsPickedUp(Guid id)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.ShippingsRepository;
                var shipping = await repo.Find(id);

                shipping.ChangeStateToPickedUp();
                await UpdateShipping(shipping, unitOfWork);
            }
        }

        public async Task PublishShipping(Guid id)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.ShippingsRepository;
                var shipping = await repo.Find(id);

                shipping.PublishShippingOrder();
                await UpdateShipping(shipping, unitOfWork);

                _shippingPublishedEventEmitter.Emit(
                    new ShippingPublishedIntegrationEvent() { ShippingInformation = shipping.MapToInterfaces() });
            }
        }

        private async Task UpdateShipping(Shipping shipping, IUnitOfWork unitOfWork)
        {
            var repo = await unitOfWork.ShippingsRepository;
            await repo.Update(shipping.Id, shipping);
            await unitOfWork.Commit();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGO.EventBus.Abstractions;
using IUGO.Turns.Core.Specifications;
using IUGO.Turns.Core.TurnAggreate;
using IUGO.Turns.Infrastructure.Data;
using IUGO.Turns.Infrastructure.Data.ServiceFabricStorage;
using IUGO.Turns.Infrastructure.Integration;
using IUGO.Turns.Services.Interface;
using IUGO.Turns.Services.Interface.Integration;
using IUGO.Turns.Services.Interface.Models;
using IUGO.Turns.Services.Mappers;
using IUGO.Vehicles.Services.Interfaces;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace IUGO.Turns.Services
{
    public class TurnServices : StatefulService, ITurnService
    {
        //private IUnitOfWork unitOfWork;
        private CancellationToken _cancellationToken;
        private readonly IVehiclesServices _vehicleService;
        private readonly EventEmitter<TurnAssignedMessageIntegrationEvent> _turnAssignedEmitter;


        /// <inheritdoc />
        protected override Task RunAsync(CancellationToken cancellationToken)
        {
            // _unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager);
            _cancellationToken = cancellationToken;
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(this.CreateServiceRemotingListener)
            };
        }

        public TurnServices(StatefulServiceContext serviceContext, IVehiclesServices vehicleService, EventEmitter<TurnAssignedMessageIntegrationEvent> turnAssignedEmitter) : base(serviceContext)
        {
            this._vehicleService = vehicleService;
            _turnAssignedEmitter = turnAssignedEmitter;
        }

        public async Task<OutputTurnModel> FindTurn(Guid id)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.TurnsRepository;
                var coreTurn = await repo.Find(id);
                var outputModel = coreTurn.MapToOutputTurnModel();

                return outputModel;
            }
        }

        public async Task<OutputTurnModel> CreateTurn(TurnInputModel turnInputModel)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.TurnsRepository;

                var vehicle = await _vehicleService.FindVehicle(turnInputModel.VehicleId);

                var turn = new Turn(turnInputModel.AvailableFrom
                    , turnInputModel.DriverId
                    , turnInputModel.VehicleId
                    , turnInputModel.VehicleDesignationId);

                var driverSpecification = new DriverSpecification(turn.DriverId);
                var driverIsAlreadyInTurn = (await repo.ListBySpecification(driverSpecification)).Any();
                if (driverIsAlreadyInTurn) throw new Exception("Driver is already in the queue");

                var vehicleSpecification = new VehicleSpecification(turn.VehicleId);
                var vehicleIsAlreadyInTurn = (await repo.ListBySpecification(vehicleSpecification)).Any();
                if (vehicleIsAlreadyInTurn) throw new Exception("Vehicle is already in the queue");

                await repo.Add(turn);
                await unitOfWork.Commit();


                return turn.MapToOutputTurnModel();
            }
        }

        public async Task DeleteTurn(Guid id)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.TurnsRepository;
                var turnToBeDeleted = await repo.Find(id);
                await repo.Delete(turnToBeDeleted);

                await unitOfWork.Commit();
            }
        }

        public async Task AddDestination(Guid turnId, string destinationId)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.TurnsRepository;
                var turn = await repo.Find(turnId);
                turn.AddDestiniation(destinationId);

                await repo.Update(turnId, turn);
                await unitOfWork.Commit();
            }
        }

        public async Task AddOrigin(Guid turnId, string originId)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.TurnsRepository;
                var turn = await repo.Find(turnId);
                turn.AddOrigin(originId);

                await repo.Update(turnId, turn);
                await unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<OutputTurnModel>> FindTurnsNotAssignedBy(IEnumerable<string> destinationIds, IEnumerable<string> originIds, DateTime pickUpDate, IEnumerable<string> vehicleDesignationIds)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var destinationSpecification = new GoingToSpecification(destinationIds);
                var originSpecification = new PickingUpFromSpecification(originIds);
                var pickUpDateSpecification = new PickUpDateSpecification(pickUpDate);
                var vehicleDesignationSpecificaion = new VehicleDesignationSpecification(vehicleDesignationIds);
                var turnAssignedSpecification = new TurnNotAssignedSpecification();

                var shippingSpecification = destinationSpecification
                    .And(originSpecification)
                    .And(pickUpDateSpecification)
                    .And(vehicleDesignationSpecificaion)
                    .And(turnAssignedSpecification);

                var repo = await unitOfWork.TurnsRepository;
                var availableTurns = await repo.ListBySpecification(shippingSpecification);

                var outputTurns = availableTurns.Select(turn => turn.MapToOutputTurnModel());

                return outputTurns;
            }
        }

        public async Task AssignTurnToShippingService(Guid turnId, string shippingServiceId)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.TurnsRepository;
                var turn = await repo.Find(turnId);
                turn.AssignShipping(shippingServiceId);
                await repo.Update(turnId, turn);
                await unitOfWork.Commit();

                var message = new TurnAssignedMessageIntegrationEvent()
                {
                    TurnId = turn.Id.ToString(),
                    DriverId = turn.DriverId,
                    VehicleId = turn.VehicleId,
                    ShippingServiceId = shippingServiceId,
                    VehicleDesignationId = turn.VehicleDesignationId
                };

                _turnAssignedEmitter.Emit(message);
            }
        }

        public async Task AcceptShippingOffer(Guid turnId, string shippingServiceId)
        {
            using (var unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager))
            {
                var repo = await unitOfWork.TurnsRepository;
                var turn = await repo.Find(turnId);
                turn.AcceptShippingOffer(shippingServiceId);
                await repo.Update(turnId, turn);

                await unitOfWork.Commit();
            }
        }
    }
}

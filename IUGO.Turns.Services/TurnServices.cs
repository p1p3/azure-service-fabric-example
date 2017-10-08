using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IUGO.Turns.Core.Specifications;
using IUGO.Turns.Core.TurnAggreate;
using IUGO.Turns.Infrastructure.Data;
using IUGO.Turns.Infrastructure.Data.ServiceFabricStorage;
using IUGO.Turns.Services.Interface;
using IUGO.Turns.Services.Interface.Models;
using IUGO.Turns.Services.Mappers;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace IUGO.Turns.Services
{
    public class TurnServices : StatefulService, ITurnService
    {
        private IUnitOfWork _unitOfWork;
        private CancellationToken _cancellationToken;

        /// <inheritdoc />
        protected override Task RunAsync(CancellationToken cancellationToken)
        {
            _unitOfWork = new UnitOfWorkReliableStateManager(this.StateManager);
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

        public TurnServices(StatefulServiceContext serviceContext) : base(serviceContext)
        {
        }

        public async Task<OutputTurnModel> FindTurn(Guid id)
        {
            var repo = await _unitOfWork.TurnsRepository;
            var coreTurn = await repo.FindTurn(id);
            var outputModel = coreTurn.MapToOutputTurnModel();

            return outputModel;
        }

        public async Task<OutputTurnModel> CreateTurn(TurnInputModel turnInputModel)
        {
            var repo = await _unitOfWork.TurnsRepository;
            var turn = new Turn(turnInputModel.AvailableFrom
                , turnInputModel.DriverId
                , turnInputModel.VehicleId);

            var driverSpecification = new DriverSpecification(turn.DriverId);
            var driverIsAlreadyInTurn = (await repo.ListBySpecification(driverSpecification)).Any();
            if (driverIsAlreadyInTurn) throw new Exception("Driver is already in the queue");

            var vehicleSpecification = new VehicleSpecification(turn.VehicleId);
            var vehicleIsAlreadyInTurn = (await repo.ListBySpecification(vehicleSpecification)).Any();
            if (vehicleIsAlreadyInTurn) throw new Exception("Vehicle is already in the queue");

            await repo.AddTurn(turn);
            await _unitOfWork.Commit();

            return turn.MapToOutputTurnModel();
        }

        public async Task DeleteTurn(Guid id)
        {

            var repo = await _unitOfWork.TurnsRepository;
            var turnToBeDeleted = await repo.FindTurn(id);
            await repo.DeleteTurn(turnToBeDeleted);

            await _unitOfWork.Commit();
        }

        public async Task AddDestination(Guid turnId, string destinationId)
        {
            var repo = await _unitOfWork.TurnsRepository;
            var turn = await repo.FindTurn(turnId);
            turn.AddDestiniation(destinationId);

            await repo.UpdateTurn(turnId, turn);
            await _unitOfWork.Commit();
        }

        public async Task AddOrigin(Guid turnId, string originId)
        {
            var repo = await _unitOfWork.TurnsRepository;
            var turn = await repo.FindTurn(turnId);
            turn.AddOrigin(originId);

            await repo.UpdateTurn(turnId, turn);
            await _unitOfWork.Commit();
        }

        public async Task<IEnumerable<OutputTurnModel>> FindTurnsBy(IEnumerable<string> destinationIds, IEnumerable<string> originIds, DateTime pickUpDate)
        {
            var destinationSpecification = new GoingToSpecification(destinationIds);
            var originSpecification = new PickingUpFromSpecification(originIds);
            var pickUpDateSpecification = new PickUpDateSpecification(pickUpDate);

            var shippingSpecification = destinationSpecification.And(originSpecification).And(pickUpDateSpecification);

            var repo = await _unitOfWork.TurnsRepository;
            var availableTurns = await repo.ListBySpecification(shippingSpecification);

            var outputTurns = availableTurns.Select(turn => turn.MapToOutputTurnModel());

            return outputTurns;

        }
    }
}

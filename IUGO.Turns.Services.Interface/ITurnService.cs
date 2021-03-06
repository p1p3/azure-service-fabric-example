﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IUGO.Turns.Services.Interface.Models;
using Microsoft.ServiceFabric.Services.Remoting;

namespace IUGO.Turns.Services.Interface
{
    public interface ITurnService : IService
    {
        Task<OutputTurnModel> FindTurn(Guid id);
        Task<OutputTurnModel> CreateTurn(TurnInputModel turnInputModel);
        Task DeleteTurn(Guid id);

        Task AddDestination(Guid turnId, string destinationId);

        Task AddOrigin(Guid turnId, string originId);

        Task<IEnumerable<OutputTurnModel>> FindTurnsNotAssignedBy(IEnumerable<string> destinationIds,
            IEnumerable<string> originIds, DateTime pickUpDate, IEnumerable<string> vehicleDesignationIds);

        Task AssignTurnToShippingService(Guid turnId, string shippingServiceId);
        Task AcceptShippingOffer(Guid turnId, string shippingServiceId);
    }
}

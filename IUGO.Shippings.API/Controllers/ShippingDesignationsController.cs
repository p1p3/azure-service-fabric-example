using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IUGO.Shippings.Services.Interfaces;
using IUGO.Shippings.Services.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;

namespace IUGO.Shippings.API.Controllers
{
    [Produces("application/json")]
    [Route("api/shipping/{shippingId}/designations")]
    public class ShippingDesignationsController : Controller
    {
        private readonly IShippingService _shippingService;

        public ShippingDesignationsController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }


        [HttpGet]
        public async Task<IEnumerable<string>> Get(string shippingId)
        {
            var turn = await _shippingService.FindShipping(Guid.Parse(shippingId));
            return turn.RequiredVehicleDesignationsIds;
        }

        // POST: api/TurnDestinations
        [HttpPost]
        public Task<ShippingOutputModel> Post(string shippingId, [FromBody] string vehicleDesignationId)
        {
            return _shippingService.AddRequiredVehicleDesignation(Guid.Parse(shippingId), vehicleDesignationId);
        }

    }
}

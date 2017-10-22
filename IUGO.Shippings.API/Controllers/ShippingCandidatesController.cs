using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IUGO.Shippings.Services.Interfaces;
using IUGO.Shippings.Services.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;

namespace IUGO.Shippings.API.Controllers
{
    [Produces("application/json")]
    [Route("api/shipping/{shippingId}/candidates")]
    public class ShippingCandidatesController : Controller
    {
        private readonly IShippingService _shippingService;

        public ShippingCandidatesController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }


        [HttpGet]
        public async Task<IEnumerable<ShippingTurn>> Get(string shippingId)
        {
            var turn = await _shippingService.FindShipping(Guid.Parse(shippingId));
            return turn.Candidates;
        }

        // POST: api/TurnDestinations
        [HttpPost]
        public Task<ShippingOutputModel> Post(string shippingId, [FromBody] ShippingTurn canadidate)
        {
            return _shippingService.AddCandidate(canadidate, Guid.Parse(shippingId));
        }


    }
}

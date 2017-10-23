using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IUGO.Turns.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IUGO.Turns.API.Controllers
{
    [Produces("application/json")]
    [Route("api/turn/{turnId}/shipping")]
    public class TurnShippingController : Controller
    {
        private readonly ITurnService _turnService;

        public TurnShippingController(ITurnService turnService)
        {
            _turnService = turnService;
        }

        // POST: api/TurnDestinations
        // POST api/values
        [HttpPost]
        public Task Post(string turnId, [FromBody]string shippingServiceId)
        {
            return _turnService.AssignTurnToShippingService(Guid.Parse(turnId), shippingServiceId);
        }
    }
}

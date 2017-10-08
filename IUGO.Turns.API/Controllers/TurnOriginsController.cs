using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IUGO.Turns.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IUGO.Turns.API.Controllers
{
    [Produces("application/json")]
    [Route("api/turn/{turnId}/origins")]
    public class TurnOriginsController : Controller
    {
        private readonly ITurnService _turnService;

        public TurnOriginsController(ITurnService turnService)
        {
            _turnService = turnService;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get(string turnId)
        {
            var turn = await _turnService.FindTurn(Guid.Parse(turnId));
            return turn.OriginIds;
        }

        // POST: api/TurnDestinations
        [HttpPost]
        public Task Post(string turnId, [FromBody] string originId)
        {
            return _turnService.AddOrigin(Guid.Parse(turnId), originId);
        }
    }
}

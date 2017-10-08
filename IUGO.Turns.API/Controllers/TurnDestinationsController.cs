using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IUGO.Turns.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IUGO.Turns.API.Controllers
{
    [Produces("application/json")]
    [Route("api/turn/{turnId}/destinations")]
    public class TurnDestinationsController : Controller
    {
        private readonly ITurnService _service;

        // GET: api/TurnDestinations
        public TurnDestinationsController(ITurnService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get(string turnId)
        {
            var turn = await _service.FindTurn(Guid.Parse(turnId));
            return turn.DestiniationIds;
        }

        // POST: api/TurnDestinations
        [HttpPost]
        public Task Post(string turnId, [FromBody] string destinationId)
        {
            return _service.AddDestination(Guid.Parse(turnId), destinationId);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //TODO REMOVE DESTINATION
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IUGO.Turns.API.Models;
using IUGO.Turns.Services.Interface;
using IUGO.Turns.Services.Interface.Models;
using Microsoft.AspNetCore.Mvc;

namespace IUGO.Turns.API.Controllers
{
    [Route("api/[controller]")]
    public class TurnsController : Controller
    {
        private readonly ITurnService _turnsService;

        public TurnsController(ITurnService turnsService)
        {
            _turnsService = turnsService;
        }

        [HttpGet]
        public Task<IEnumerable<OutputTurnModel>> Get([FromQuery] string destinationId, [FromQuery] string originId, [FromQuery] DateTime pickUpDate, string vehicleDesginationId)
        {
            return _turnsService.FindTurnsBy(new[] { destinationId }, new[] { originId }, pickUpDate, new[] { vehicleDesginationId});
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Task<OutputTurnModel> Get(string id)
        {
            return _turnsService.FindTurn(Guid.Parse(id));
        }

        // POST api/values
        [HttpPost]
        public Task<OutputTurnModel> Post([FromBody]TurnInputModel turn)
        {
            return _turnsService.CreateTurn(turn);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public Task Delete(string id)
        {
            return _turnsService.DeleteTurn(Guid.Parse(id));
        }

        // POST api/values
        [HttpPut("{id}")]
        public Task Post(string id, [FromBody]string shippingServiceId)
        {
            return _turnsService.AssignTurnToShippingService(Guid.Parse(id), shippingServiceId);
        }

    }
}

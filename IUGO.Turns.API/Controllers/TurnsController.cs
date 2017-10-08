using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
    }
}

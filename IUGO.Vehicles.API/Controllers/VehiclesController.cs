using System.Collections.Generic;
using System.Threading.Tasks;
using IUGO.Vehicles.Services.Interfaces;
using IUGO.Vehicles.Services.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;

namespace IUGO.Vehicles.API.Controllers
{
    [Route("api/[controller]")]
    public class VehiclesController : Controller
    {
        private readonly IVehiclesServices _vehiclesService;

        public VehiclesController(IVehiclesServices vehiclesService)
        {
            _vehiclesService = vehiclesService;
        }

        [HttpGet]
        public Task<IEnumerable<Vehicle>> Get()
        {
            return _vehiclesService.List();
        }

        [HttpGet("{id}")]
        public Task<Vehicle> Get(string id)
        {
            return _vehiclesService.FindVehicle(id);
        }

        [HttpPost]
        public Task<Vehicle> Post([FromBody]Vehicle vehicle)
        {
            return _vehiclesService.CreateVehicle(vehicle);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

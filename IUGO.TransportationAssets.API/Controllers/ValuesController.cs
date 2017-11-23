using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IUGO.TransportationAssets.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IUGO.TransportationAssets.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IDriverService _driverService;
        private readonly IVehiclesServices _vehiclesServices;

        // GET api/values
        public ValuesController(IDriverService driverService, IVehiclesServices vehiclesServices)
        {
            _driverService = driverService;
            _vehiclesServices = vehiclesServices;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            try
            {
                await _driverService.DoSomthing();
                await _vehiclesServices.DoSomthing();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

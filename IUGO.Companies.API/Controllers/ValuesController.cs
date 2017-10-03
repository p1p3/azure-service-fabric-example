using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IUGO.Companies.Services.Aplication_Services;
using IUGO.Companies.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;

namespace IUGO.Companies.API.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ICompanyService _companyService;

        public ValuesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _companyService.AddCompany(new Company() {Id = Guid.NewGuid(), Name = "holaaa"});
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

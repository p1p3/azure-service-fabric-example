using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IUGO.Companies.Services;
using IUGO.Companies.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;

namespace IUGO.Companies.API.Controllers
{
    [Route("api/[controller]")]
    public class CompaniesController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<Company>> Get()
        {
            var companies = await _companyService.ListAll();
            return companies;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public Task Post([FromBody]string name)
        {
            return _companyService.AddCompany(new Company() { Id = Guid.NewGuid(), Name = name });
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

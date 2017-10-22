using System;
using System.Threading.Tasks;
using IUGO.Shippings.Services.Interfaces;
using IUGO.Shippings.Services.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;

namespace IUGO.Shippings.API.Controllers
{
    [Route("api/[controller]")]
    public class ShippingController : Controller
    {
        private readonly IShippingService _shippingService;

        public ShippingController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public Task<ShippingOutputModel> Get(string id)
        {
            return _shippingService.FindShipping(Guid.Parse(id));
        }

        // POST api/values
        [HttpPost]
        public Task<ShippingOutputModel> Post([FromBody]ShippingInputModel shipping)
        {
            return _shippingService.CreateShipping(shipping);
        }


        [HttpPost("{id}/publish")]
        public Task Publish(string id)
        {
            return _shippingService.PublishShipping(Guid.Parse(id));
        }


    }
}

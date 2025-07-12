using Microsoft.AspNetCore.Mvc;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/vendingmachine")]
    public abstract class VendingMachineController : ControllerBase
    {
        protected readonly IEnumerable<IProduct> _products;
 
        protected VendingMachineController(IEnumerable<IProduct> products)
        {
            _products = products;
        }

        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            return Ok(_products);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using backend.Domain;
using backend.Application;

namespace backend.API
{
    [ApiController]
    [Route("api/vendingmachine")]
    public abstract class VendingMachineController : ControllerBase
    {
        protected readonly IGetProductsQuery _getProductsQuery;
        protected readonly IBuyProductsCommand _buyCommand;
        protected VendingMachineController(
            IGetProductsQuery getProductsQuery,
            IBuyProductsCommand buyProductCommand)
        {
            _getProductsQuery = getProductsQuery;
            _buyCommand = buyProductCommand;
        }

        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            var result = _getProductsQuery.Execute();
            return Ok(result);
        }

        [HttpPost("buy")]
        public IActionResult Buy(BuyProducstRequestModel request)
        {
            var result = _buyCommand.Execute(request);
            if (result.Status == "error")
                return BadRequest(result);
            return Ok(result);
        }
    }
}
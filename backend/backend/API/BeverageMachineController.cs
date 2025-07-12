using Microsoft.AspNetCore.Mvc;
using backend.Domain;
using backend.Application;

namespace backend.API
{
    [Route("api/beveragemachine")]
    [ApiController]
    public class BeverageMachineController : VendingMachineController
    {
        public BeverageMachineController(
            [FromKeyedServices("beverage")] IGetProductsQuery getProductsQuery,
            [FromKeyedServices("beverage")] IBuyProductsCommand buyProductCommand)
            : base(getProductsQuery, buyProductCommand)
        {
        }
    }
}
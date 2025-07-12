using Microsoft.AspNetCore.Mvc;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/beveragemachine")]
    [ApiController]
    public class BeverageMachineController : VendingMachineController
    {
        public BeverageMachineController(ILogger<BeverageMachineController> logger)
            : base(
            [
                new BeverageModel { ImageUrl = "https://res.cloudinary.com/dzodpkye3/image/upload/v1752278876/cocacola_az1szy.png", Name = "Coca Cola", Quantity = 10, Price = 800m },
                new BeverageModel { ImageUrl = "https://res.cloudinary.com/dzodpkye3/image/upload/v1752278879/pepsi_qc7vz4.png", Name = "Pepsi", Quantity = 8, Price = 750m },
                new BeverageModel { ImageUrl = "https://res.cloudinary.com/dzodpkye3/image/upload/v1752278878/fanta_a2hqiq.png", Name = "Fanta", Quantity = 10, Price = 950m },
                new BeverageModel { ImageUrl = "https://res.cloudinary.com/dzodpkye3/image/upload/v1752279138/sprite_jbeejn.png", Name = "Sprite", Quantity = 15, Price = 975m }
            ])
        {
        }
    }
}
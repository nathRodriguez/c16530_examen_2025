using backend.Repository;
using backend.Application.DTOs;
using backend.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Application
{
    public class BuyBeverageCommand : IBuyProductsCommand
    {
        private readonly IVendingMachineRepository _repository;
        
        public BuyBeverageCommand([FromKeyedServices("beverage")] IVendingMachineRepository repository)
        {
            _repository = repository;
        }

        public BuyProductsResponseDto Execute(BuyProducstRequestModel buyInformation)
        {
            return _repository.ProcessPurchase(buyInformation);
        }
    }
}
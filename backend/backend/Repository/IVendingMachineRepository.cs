using backend.Domain;
using backend.Application.DTOs;
namespace backend.Repository
{
    public interface IVendingMachineRepository
    {
        IEnumerable<MoneyUnitModel> GetMoneyUnitsAvailable();
        IEnumerable<IProductModel> GetProducts();
        BuyProductsResponseDto ProcessPurchase(BuyProducstRequestModel buyRequest);
    }
}
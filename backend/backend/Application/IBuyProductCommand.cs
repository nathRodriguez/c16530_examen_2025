using backend.Application.DTOs;
using backend.Domain;

namespace backend.Application
{
    public interface IBuyProductsCommand
    {
        BuyProductsResponseDto Execute(BuyProducstRequestModel buyInformation);
    }
}

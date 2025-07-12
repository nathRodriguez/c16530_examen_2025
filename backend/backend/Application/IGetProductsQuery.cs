using backend.Application.DTOs;
using backend.Domain;

namespace backend.Application
{
    public interface IGetProductsQuery
    {
        IEnumerable<IProductModel> Execute();
    }

}

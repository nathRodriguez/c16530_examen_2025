using backend.Domain;
using backend.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Application
{
    public class GetBeveragesQuery : IGetProductsQuery
    {
        private readonly IVendingMachineRepository _repository;
        
        public GetBeveragesQuery([FromKeyedServices("beverage")] IVendingMachineRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<IProductModel> Execute()
        {
            return _repository.GetProducts();
        }
    }
}
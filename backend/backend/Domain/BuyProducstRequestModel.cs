namespace backend.Domain
{
    public class BuyProducstRequestModel
    {
        public IEnumerable<ProductInformation>? Products { get; set; }
        public IEnumerable<MoneyInformation>? Payment { get; set; }
    }

    public class ProductInformation
    {
        public required string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class MoneyInformation
    {
        public decimal Value { get; set; }
        public decimal Quantity { get; set; }
    }
}

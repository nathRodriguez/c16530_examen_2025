namespace backend.Domain
{
    public class BeverageModel : IProductModel
    {
        public required string ImageUrl { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
namespace backend.Models
{
    public class BeverageModel : IProduct
    {
        public required string ImageUrl { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
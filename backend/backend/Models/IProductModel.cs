namespace backend.Models
{
    public interface IProduct
    {
        string ImageUrl { get; set; }
        string Name { get; set; }
        decimal Price { get; set; }
        int Quantity { get; set; }
    }
}
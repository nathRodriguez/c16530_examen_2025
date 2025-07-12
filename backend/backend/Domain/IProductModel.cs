namespace backend.Domain
{
    public interface IProductModel
    {
        string ImageUrl { get; set; }
        string Name { get; set; }
        decimal Price { get; set; }
        int Quantity { get; set; }
    }
}
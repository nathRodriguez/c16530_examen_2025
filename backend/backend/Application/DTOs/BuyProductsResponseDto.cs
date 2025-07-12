using backend.Domain;

namespace backend.Application.DTOs
{
    public class BuyProductsResponseDto
    {
        public int TotalCost { get; set; }
        public int TotalPayment { get; set; }
        public List<ProductInformation> PurchasedProducts { get; set; } = [];
        public List<MoneyInformation> PaymentBreakdown { get; set; } = [];
        public string Status { get; set; } = string.Empty; // "success", "error", "out_of_service"
        public string? Message { get; set; } // Error message when status is "error"
        public int ChangeAmount { get; set; }
        public IEnumerable<ChangeBreakdownDto> ChangeBreakdown { get; set; } = [];
        public IEnumerable<BeverageModel>? Beverages { get; set; } // List of beverages updated when status is "success"
    }
    public class ChangeBreakdownDto
    {
        public int Value { get; set; }
        public int Quantity { get; set; }
    }
}
using backend.Domain;
using backend.Application.DTOs;

namespace backend.Repository
{
    public class BeverageMachineRepository : IVendingMachineRepository
    {
        private IEnumerable<MoneyUnitModel> moneyUnitsAvailable = [];
        private IEnumerable<BeverageModel> beverages { get; set; } = [];

        public BeverageMachineRepository()
        {
            moneyUnitsAvailable =
            [
                new MoneyUnitModel { Value = 500, Amount = 20 },
                new MoneyUnitModel { Value = 100, Amount = 30 },
                new MoneyUnitModel { Value = 50, Amount = 50 },
                new MoneyUnitModel { Value = 25, Amount = 25 }
            ];

            beverages =
            [
                new BeverageModel { ImageUrl = "https://res.cloudinary.com/dzodpkye3/image/upload/v1752278876/cocacola_az1szy.png", Name = "Coca Cola", Quantity = 10, Price = 800m },
                new BeverageModel { ImageUrl = "https://res.cloudinary.com/dzodpkye3/image/upload/v1752278879/pepsi_qc7vz4.png", Name = "Pepsi", Quantity = 8, Price = 750m },
                new BeverageModel { ImageUrl = "https://res.cloudinary.com/dzodpkye3/image/upload/v1752278878/fanta_a2hqiq.png", Name = "Fanta", Quantity = 10, Price = 950m },
                new BeverageModel { ImageUrl = "https://res.cloudinary.com/dzodpkye3/image/upload/v1752279138/sprite_jbeejn.png", Name = "Sprite", Quantity = 15, Price = 975m }
            ];
        }

        public IEnumerable<MoneyUnitModel> GetMoneyUnitsAvailable()
        {
            return moneyUnitsAvailable;
        }

        public IEnumerable<IProductModel> GetProducts()
        {
            return beverages;
        }

        public BuyProductsResponseDto ProcessPurchase(BuyProducstRequestModel buyRequest)
        {
            try
            {
                var serviceCheck = CheckMachineServiceStatus();
                if (!serviceCheck.IsAvailable)
                {
                    return CreateOutOfServiceResponse(serviceCheck.Message);
                }

                var requestValidation = ValidateRequest(buyRequest);
                if (!requestValidation.IsValid)
                {
                    return CreateErrorResponse("error", requestValidation.ErrorMessage);
                }

                var transactionAmounts = CalculateTransactionAmounts(buyRequest);
                if (!transactionAmounts.IsValid)
                {
                    return CreateErrorResponse("error", transactionAmounts.ErrorMessage);
                }

                var changeResult = CalculateChange(transactionAmounts.ChangeNeeded);
                if (!changeResult.CanProvideChange)
                {
                    return CreateErrorResponse("error", "Fallo al realizar la compra");
                }

                ProcessTransactionUpdates(buyRequest, changeResult.MoneyUnitsAfterChange);

                return CreateSuccessResponse(transactionAmounts, changeResult, buyRequest);
            }
            catch (Exception)
            {
                return CreateErrorResponse("error", "Fallo al realizar la compra");
            }
        }

        private (bool IsAvailable, string Message) CheckMachineServiceStatus()
        {
            bool hasCoinsLeft = moneyUnitsAvailable.Any(m => m.Amount > 0);
            return (hasCoinsLeft, hasCoinsLeft ? "" : "Fuera de servicio");
        }

        private (bool IsValid, string ErrorMessage) ValidateRequest(BuyProducstRequestModel buyRequest)
        {
            if (buyRequest.Products == null)
            {
                return (false, "No se especificaron productos para comprar.");
            }

            var unavailableProducts = new List<string>();
            var beveragesList = beverages.ToList();

            foreach (var requestedProduct in buyRequest.Products)
            {
                var beverage = beveragesList.FirstOrDefault(b => b.Name == requestedProduct.Name);
                if (beverage == null || requestedProduct.Quantity <= 0 || requestedProduct.Quantity > beverage.Quantity)
                {
                    unavailableProducts.Add(requestedProduct.Name);
                }
            }

            if (unavailableProducts.Any())
            {
                return (false, $"No hay suficientes unidades de: {string.Join(", ", unavailableProducts)}");
            }

            return (true, "");
        }

        private (bool IsValid, string ErrorMessage, decimal TotalCost, decimal TotalPayment, decimal ChangeNeeded) CalculateTransactionAmounts(BuyProducstRequestModel buyRequest)
        {
            decimal totalCost = 0;
            var beveragesList = beverages.ToList();

            foreach (var requestedProduct in buyRequest.Products)
            {
                var beverage = beveragesList.First(b => b.Name == requestedProduct.Name);
                totalCost += beverage.Price * requestedProduct.Quantity;
            }

            decimal totalPayment = (buyRequest.Payment ?? Enumerable.Empty<MoneyInformation>()).Sum(p => p.Value * p.Quantity);

            if (totalPayment < totalCost)
            {
                return (false, "Dinero insuficiente para realizar la compra.", 0, 0, 0);
            }

            decimal changeNeeded = totalPayment - totalCost;
            return (true, "", totalCost, totalPayment, changeNeeded);
        }

        private (bool CanProvideChange, List<ChangeBreakdownDto> ChangeBreakdown, List<MoneyUnitModel> MoneyUnitsAfterChange) CalculateChange(decimal changeNeeded)
        {
            var moneyUnitsForChange = moneyUnitsAvailable.ToList();
            var changeBreakdown = new List<ChangeBreakdownDto>();
            decimal remainingChange = changeNeeded;
            var sortedMoneyUnits = moneyUnitsForChange.OrderByDescending(m => m.Value).ToList();

            foreach (var moneyUnit in sortedMoneyUnits)
            {
                if (remainingChange <= 0) break;

                int coinsNeeded = (int)(remainingChange / moneyUnit.Value);
                int coinsToGive = Math.Min(coinsNeeded, moneyUnit.Amount);

                if (coinsToGive > 0)
                {
                    changeBreakdown.Add(new ChangeBreakdownDto 
                    { 
                        Value = moneyUnit.Value, 
                        Quantity = coinsToGive 
                    });
                    
                    moneyUnit.Amount -= coinsToGive;
                    remainingChange -= coinsToGive * moneyUnit.Value;
                }
            }

            bool canProvideChange = remainingChange <= 0;
            return (canProvideChange, changeBreakdown, moneyUnitsForChange);
        }

        private void ProcessTransactionUpdates(BuyProducstRequestModel buyRequest, List<MoneyUnitModel> updatedMoneyUnits)
        {
            var beveragesList = beverages.ToList();
            foreach (var requestedProduct in buyRequest.Products)
            {
                var beverage = beveragesList.First(b => b.Name == requestedProduct.Name);
                beverage.Quantity -= requestedProduct.Quantity;
            }

            beverages = beveragesList;
            moneyUnitsAvailable = updatedMoneyUnits;
        }

        private BuyProductsResponseDto CreateErrorResponse(string status, string message)
        {
            return new BuyProductsResponseDto
            {
                Status = status,
                Message = message,
                TotalCost = 0,
                TotalPayment = 0,
                ChangeAmount = 0,
                ChangeBreakdown = new List<ChangeBreakdownDto>(),
                PurchasedProducts = new List<ProductInformation>(),
                PaymentBreakdown = new List<MoneyInformation>(),
                Beverages = GetCurrentBeveragesState()
            };
        }

        private BuyProductsResponseDto CreateOutOfServiceResponse(string message)
        {
            return new BuyProductsResponseDto
            {
                Status = "out_of_service",
                Message = message,
                TotalCost = 0,
                TotalPayment = 0,
                ChangeAmount = 0,
                ChangeBreakdown = new List<ChangeBreakdownDto>(),
                PurchasedProducts = new List<ProductInformation>(),
                PaymentBreakdown = new List<MoneyInformation>(),
                Beverages = GetCurrentBeveragesState()
            };
        }

        private BuyProductsResponseDto CreateSuccessResponse(
            (bool IsValid, string ErrorMessage, decimal TotalCost, decimal TotalPayment, decimal ChangeNeeded) transactionAmounts,
            (bool CanProvideChange, List<ChangeBreakdownDto> ChangeBreakdown, List<MoneyUnitModel> MoneyUnitsAfterChange) changeResult,
            BuyProducstRequestModel buyRequest)
        {
            return new BuyProductsResponseDto
            {
                Status = "success",
                Message = "",
                TotalCost = (int)transactionAmounts.TotalCost,
                TotalPayment = (int)transactionAmounts.TotalPayment,
                ChangeAmount = (int)transactionAmounts.ChangeNeeded,
                ChangeBreakdown = changeResult.ChangeBreakdown,
                PurchasedProducts = (buyRequest.Products ?? Enumerable.Empty<ProductInformation>()).ToList(),
                PaymentBreakdown = buyRequest.Payment?.ToList() ?? new List<MoneyInformation>(),
                Beverages = GetCurrentBeveragesState()
            };
        }

        private List<BeverageModel> GetCurrentBeveragesState()
        {
            return beverages.Select(b => new BeverageModel
            {
                Name = b.Name,
                ImageUrl = b.ImageUrl,
                Price = (int)b.Price,
                Quantity = b.Quantity
            }).ToList();
        }
    }
}
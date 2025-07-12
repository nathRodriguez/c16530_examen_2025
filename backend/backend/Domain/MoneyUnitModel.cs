namespace backend.Domain
{
    public class MoneyUnitModel
    {
        public int Value { get; set; }
        public int Amount { get; set; } // How many coins/bills of this value are available
    }
}
namespace MarketPriceAPI.Data.Entities
{
    public class Asset
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string Kind { get; set; }
        public decimal TickSize { get; set; }
        public string Currency { get; set; }
        public string? BaseCurrency { get; set; }
    }
}

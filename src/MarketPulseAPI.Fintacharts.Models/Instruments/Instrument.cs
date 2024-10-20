using System.Text.Json.Serialization;

namespace MarketPulseAPI.Fintacharts.Models.Instruments
{
    public class Instrument
    {
        [JsonConstructor]
        public Instrument(Guid id, string symbol, string kind, string description, decimal tickSize,
            string currency, string baseCurrency)
        {
            Id = id;
            Symbol = symbol;
            Kind = kind;
            Description = description;
            TickSize = tickSize;
            Currency = currency;
            BaseCurrency = baseCurrency;
        }

        public Guid Id { get; }
        public string Symbol { get; }
        public string Kind { get; }
        public string Description { get; }
        public decimal TickSize { get; }
        public string Currency { get; }
        public string BaseCurrency { get; }
    }
}

namespace MarketPulseAPI.Fintacharts.Models.Dtos
{
    public class HistoricalPrice
    {
        public DateTime Timestamp { get; set; } // "t"
        public decimal Open { get; set; } // "o"
        public decimal High { get; set; } // "h"
        public decimal Low { get; set; } // "l"
        public decimal Close { get; set; } // "c"
        public long Volume { get; set; } // "v"
    }
}

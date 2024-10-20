using System.Text.Json;
using System.Text.Json.Serialization;

namespace MarketPulseAPI.Fintacharts.Models.Bars
{
    public class Bar
    {
        [JsonConstructor]
        public Bar(DateTime timestamp, decimal open, decimal high, decimal low, decimal close, long volume)
        {
            Timestamp = timestamp;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        [JsonPropertyName("t")]
        public DateTime Timestamp { get; } // "t"
        [JsonPropertyName("o")]
        public decimal Open { get; } // "o"
        [JsonPropertyName("h")]
        public decimal High { get; } // "h"
        [JsonPropertyName("l")]
        public decimal Low { get; } // "l"
        [JsonPropertyName("c")]
        public decimal Close { get; } // "c"
        [JsonPropertyName("v")]
        public long Volume { get; } // "v"
    }
}

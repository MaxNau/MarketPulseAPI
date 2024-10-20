using MarketPulseAPI.Util;
using System.Text.Json;

namespace MarketPulseAPI.Fintacharts.Models.Queries
{
    public class InstrumentsQuery
    {
        public InstrumentsQuery(string? provider = null, string? kind = null, string? symbol = null,
            int? page = null, int? size = null)
        {
            Provider = provider;
            Kind = kind;
            Symbol = symbol;
            Page = page;
            Size = size;
        }

        public string? Provider { get; }
        public string? Kind { get; }
        public string? Symbol { get; }
        public int? Page { get; }
        public int? Size { get; }

        public string ToQueryString()
        {
            return QueryStringBuilder.Build(
                (JsonNamingPolicy.CamelCase.ConvertName(nameof(Provider)), Provider),
                (JsonNamingPolicy.CamelCase.ConvertName(nameof(Kind)), Kind),
                (JsonNamingPolicy.CamelCase.ConvertName(nameof(Symbol)), Symbol),
                (JsonNamingPolicy.CamelCase.ConvertName(nameof(Page)), Page?.ToString()),
                (JsonNamingPolicy.CamelCase.ConvertName(nameof(Size)), Size?.ToString())
            );
        }
    }
}

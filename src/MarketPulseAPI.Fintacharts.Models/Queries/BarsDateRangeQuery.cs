using MarketPulseAPI.Util;
using System.Text.Json;

namespace MarketPulseAPI.Fintacharts.Models.Queries
{
    public class BarsDateRangeQuery
    {
        public BarsDateRangeQuery(string instrumentId, string provider, int interval,
            string periodicity, DateTime startDate, DateTime? endDate)
        {
            InstrumentId = instrumentId;
            Provider = provider;
            Interval = interval;
            Periodicity = periodicity;
            EndDate = endDate;
            StartDate = startDate;
        }

        public string InstrumentId { get; }
        public string Provider { get; }
        public int Interval { get; }
        public string Periodicity { get; }
        public DateTime? EndDate { get; }
        public DateTime StartDate { get; }

        public string ToQueryString()
        {
            return QueryStringBuilder.Build(
                (JsonNamingPolicy.CamelCase.ConvertName(nameof(InstrumentId)), InstrumentId),
                (JsonNamingPolicy.CamelCase.ConvertName(nameof(Provider)), Provider),
                (JsonNamingPolicy.CamelCase.ConvertName(nameof(Interval)), Interval.ToString()),
                (JsonNamingPolicy.CamelCase.ConvertName(nameof(Periodicity)), Periodicity),
                (JsonNamingPolicy.CamelCase.ConvertName(nameof(EndDate)), EndDate?.ToString("yyyy-MM-dd")),
                (JsonNamingPolicy.CamelCase.ConvertName(nameof(StartDate)), StartDate.ToString("yyyy-MM-dd"))
            );
        }
    }
}

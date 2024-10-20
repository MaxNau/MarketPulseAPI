using MarketPulseAPI.Fintacharts.Models.Bars;
using MarketPulseAPI.Fintacharts.Models.Instruments;
using MarketPulseAPI.Fintacharts.Models.Queries;
using MarketPulseAPI.Fintacharts.Models.ResponseWrappers;

namespace MarketPulseAPI.Interfaces.Clients.Fintacharts
{
    public interface IFintachartsHttpClient
    {
        Task<FintachartsPagedResponse<Instrument>?> GetInstrumentsAsync(InstrumentsQuery? instrumentsQuery = null, CancellationToken cancellationToken = default);
        Task<FintachartsResponse<Bar>?> GetBarsAsync(BarsDateRangeQuery barsDateRangeQuery, CancellationToken cancellationToken = default);
    }
}

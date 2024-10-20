using MarketPulseAPI.Fintacharts.Models.Dtos;
using System.Net.WebSockets;

namespace MarketPulseAPI.Interfaces.Services.Mid
{
    public interface IAssetsPricesService
    {
        Task<IEnumerable<HistoricalPrice>> GetHistoricalPricesAsync(HistoricalPriceQuery historicalPriceQuery, CancellationToken cancellationToken = default);
        Task GetRealTimePricesAsync(WebSocket webSocket, Guid assetId, CancellationToken cancellationToken = default);
    }
}

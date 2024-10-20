using AutoMapper;
using MarketPulseAPI.Fintacharts.Models.Dtos;
using MarketPulseAPI.Fintacharts.Models.Queries;
using MarketPulseAPI.Interfaces.Clients.Fintacharts;
using MarketPulseAPI.Interfaces.Services.Mid;
using System.Net.WebSockets;
using System.Text;

namespace MarketPulseAPI.Services.Mid
{
    public class AssetsPricesService : IAssetsPricesService
    {
        private readonly IMapper _mapper;
        private readonly IFintachartsHttpClient _fintachartsHttpClient;
        private readonly IFintachartsWebSocketClient _fintachartsWebSocketClient;
        public AssetsPricesService(
            IMapper mapper,
            IFintachartsHttpClient fintachartsHttpClient,
            IFintachartsWebSocketClient fintachartsWebSocketClient)
        {
            _mapper = mapper;
            _fintachartsHttpClient = fintachartsHttpClient;
            _fintachartsWebSocketClient = fintachartsWebSocketClient;
        }

        public async Task<IEnumerable<HistoricalPrice>> GetHistoricalPricesAsync(HistoricalPriceQuery historicalPriceQuery, CancellationToken cancellationToken = default)
        {
            var barsDateRangeQuery = _mapper.Map<BarsDateRangeQuery>(historicalPriceQuery);
            var response = await _fintachartsHttpClient.GetBarsAsync(barsDateRangeQuery, cancellationToken);

            return response == null ? [] : _mapper.Map<IEnumerable<HistoricalPrice>>(response.Data);
        }

        public async Task GetRealTimePricesAsync(WebSocket webSocket, Guid assetId, CancellationToken cancellationToken = default)
        {
            await _fintachartsWebSocketClient.ConnectAsync(cancellationToken);

            await _fintachartsWebSocketClient.SubscribeAsync(assetId.ToString(), cancellationToken);

            while (webSocket.State == WebSocketState.Open && _fintachartsWebSocketClient.State == WebSocketState.Open)
            {
                var message = await _fintachartsWebSocketClient.ReceiveMessageAsync(cancellationToken);
                await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true, cancellationToken);
            }

            await _fintachartsWebSocketClient.CloseAsync(CancellationToken.None);
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
        }
    }
}

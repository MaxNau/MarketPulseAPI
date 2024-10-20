using MarketPriceAPI.Configurations.Settings;
using MarketPulseAPI.Fintacharts.Models;
using MarketPulseAPI.Interfaces.Clients.Fintacharts;
using MarketPulseAPI.Services.TokenStores;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace MarketPulseAPI.Services.Clients.Fintacharts
{
    public class FintachartsWebSocketClient : IFintachartsWebSocketClient
    {
        private readonly ITokenStore<FintachartsToken> _tokenStore;
        private ClientWebSocket _clientWebSocket;
        private readonly FintachartsWebSocketsSettings _settings;

        public FintachartsWebSocketClient(ITokenStore<FintachartsToken> tokenStore,
            FintachartsWebSocketsSettings fintachartsWebSocketsSettings)
        {
            _tokenStore = tokenStore;
            _clientWebSocket = new ClientWebSocket();
            _settings = fintachartsWebSocketsSettings;
        }

        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            var token = (_tokenStore.Token?.Token) ?? throw new InvalidOperationException("Token is missing");
            await _clientWebSocket.ConnectAsync(new Uri($"{_settings.WebSocketUri}/api/streaming/ws/v1/realtime?token=" + token), cancellationToken);
        }

        public async Task SubscribeAsync(string instrumentId, CancellationToken cancellationToken = default)
        {
            var subscriptionMessage = new
            {
                type = "l1-subscription",
                id = "1",
                instrumentId,
                provider = "simulation",
                subscribe = true,
                kinds = new[] { "ask", "bid", "last" }
            };

            var subscriptionJson = JsonSerializer.Serialize(subscriptionMessage);
            await _clientWebSocket.SendAsync(Encoding.UTF8.GetBytes(subscriptionJson), WebSocketMessageType.Text, true, cancellationToken);
        }

        public async Task<string> ReceiveMessageAsync(CancellationToken cancellationToken = default)
        {
            var buffer = new byte[1024 * 4];
            var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }

        public async Task CloseAsync(CancellationToken cancellationToken = default)
        {
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
        }

        public WebSocketState State => _clientWebSocket?.State ?? WebSocketState.None;
    }
}

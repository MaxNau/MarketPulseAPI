using System.Net.WebSockets;

namespace MarketPulseAPI.Interfaces.Clients.Fintacharts
{
    public interface IFintachartsWebSocketClient
    {
        WebSocketState State { get; }
        Task ConnectAsync(CancellationToken cancellationToken = default);
        Task SubscribeAsync(string instrumentId, CancellationToken cancellationToken = default);
        Task<string> ReceiveMessageAsync(CancellationToken cancellationToken = default);     
        Task CloseAsync(CancellationToken cancellationToken = default);
    }
}

namespace MarketPriceAPI.Configurations.Settings
{
    public class FintachartsWebSocketsSettings
    {
        public FintachartsWebSocketsSettings(string webSocketUri)
        {
            WebSocketUri = webSocketUri;
        }

        public string WebSocketUri { get; }
    }
}

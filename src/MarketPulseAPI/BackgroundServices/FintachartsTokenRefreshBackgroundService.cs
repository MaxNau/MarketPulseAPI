
using MarketPulseAPI.Fintacharts.Models;
using MarketPulseAPI.Services.Clients.Fintacharts;
using MarketPulseAPI.Services.TokenStores;

namespace MarketPulseAPI.BackgroundServices
{
    public class FintachartsTokenRefreshBackgroundService : BackgroundService
    {
        private readonly FintachartsTokenClient _tokenClient;
        private readonly ITokenStore<FintachartsToken> _tokenStore;

        private readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(5); // Adjust as necessary

        public FintachartsTokenRefreshBackgroundService(FintachartsTokenClient tokenClient, ITokenStore<FintachartsToken> tokenStore)
        {
            _tokenClient = tokenClient;
            _tokenStore = tokenStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Get the current token
                    var token = await _tokenClient.GetAccessTokenAsync(stoppingToken);

                    if (token != null)
                    {
                        _tokenStore.SetToken(token.access_token, TimeSpan.FromSeconds(token.expires_in));
                    }

                    // Calculate time until token expiration
                    var expirationTime = _tokenStore.Token?.ExpirationTime;
                    if (expirationTime.HasValue)
                    {
                        var delay = (expirationTime.Value - DateTime.UtcNow).TotalMilliseconds - 60000; // Refresh 1 minute before expiration
                        if (delay > 0)
                        {
                            await Task.Delay((int)delay, stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions as needed (logging, retries, etc.)
                    Console.WriteLine($"Error refreshing token: {ex.Message}");
                }
            }
        }
    }
}

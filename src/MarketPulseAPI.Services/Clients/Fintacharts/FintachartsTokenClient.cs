using MarketPriceAPI.Configurations.Settings;
using System.Text.Json;

namespace MarketPulseAPI.Services.Clients.Fintacharts
{
    public class FintachartsTokenClient
    {
        private readonly HttpClient _httpClient;
        private readonly FintachartsAuthSettings _settings;

        public FintachartsTokenClient(HttpClient httpClient, FintachartsAuthSettings fintachartsAuthSettings)
        {
            _httpClient = httpClient;
            _settings = fintachartsAuthSettings;
        }

        public async Task<TokenResponse?> GetAccessTokenAsync(CancellationToken cancellationToken)
        {

            var requestBody = new Dictionary<string, string>
            {
                { "grant_type", _settings.GrantType },
                { "client_id", _settings.ClientId },
                { "username", _settings.Username },
                { "password", _settings.Password }
            };
            var response = await _httpClient.PostAsync(
                "identity/realms/fintatech/protocol/openid-connect/token",
                new FormUrlEncodedContent(requestBody),
                cancellationToken
            );

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content);
            return tokenResponse;
        }

        public class TokenResponse
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }
    }
}

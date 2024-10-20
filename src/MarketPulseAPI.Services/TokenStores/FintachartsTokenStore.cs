using MarketPulseAPI.Fintacharts.Models;

namespace MarketPulseAPI.Services.TokenStores
{
    public class FintachartsTokenStore : ITokenStore<FintachartsToken>
    {
        private FintachartsToken? _token;
        public FintachartsToken? Token => _token;
        public bool IsExpired => _token == null || DateTime.UtcNow >= _token.ExpirationTime;

        public void SetToken(string token, TimeSpan expirationDuration)
        {
            var expirationTime = DateTime.UtcNow.Add(expirationDuration);
            var newTokenInfo = new FintachartsToken(token, expirationTime);
            Interlocked.Exchange(ref _token, newTokenInfo);
        }
    }
}

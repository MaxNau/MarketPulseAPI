namespace MarketPulseAPI.Services.TokenStores
{
    public interface ITokenStore<T>
    {
        T? Token { get; }
        bool IsExpired { get; }
        void SetToken(string token, TimeSpan expirationDuration);
    }
}

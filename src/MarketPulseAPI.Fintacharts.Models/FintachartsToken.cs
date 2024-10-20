namespace MarketPulseAPI.Fintacharts.Models
{
    public class FintachartsToken
    {
        public FintachartsToken(string token, DateTime expirationTime)
        {
            Token = token;
            ExpirationTime = expirationTime;
        }

        public string Token { get; }
        public DateTime ExpirationTime { get; }
    }
}

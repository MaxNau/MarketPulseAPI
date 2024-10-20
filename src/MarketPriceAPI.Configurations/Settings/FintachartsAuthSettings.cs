namespace MarketPriceAPI.Configurations.Settings
{
    public class FintachartsAuthSettings
    {
        public FintachartsAuthSettings(string grantType, string clientId, string username, string password)
        {
            GrantType = grantType;
            ClientId = clientId;
            Username = username;
            Password = password;
        }

        public string GrantType { get; }
        public string ClientId { get; }
        public string Username { get; }
        public string Password { get; }
    }
}

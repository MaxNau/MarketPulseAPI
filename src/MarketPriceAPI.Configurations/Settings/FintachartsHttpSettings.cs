namespace MarketPriceAPI.Configurations.Settings
{
    public class FintachartsHttpSettings
    {
        public FintachartsHttpSettings(Uri baseUri)
        {
            BaseUri = baseUri;
        }

        public Uri BaseUri { get; }
    }
}

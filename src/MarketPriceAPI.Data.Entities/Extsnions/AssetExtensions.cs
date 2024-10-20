using MarketPulseAPI.Fintacharts.Models.Instruments;

namespace MarketPriceAPI.Data.Entities.Extsnions
{
    public static class AssetExtensions
    {
        public static bool HasChanged(this Asset existing, Instrument updated)
        {
            return existing.Symbol != updated.Symbol ||
                   existing.Description != updated.Description ||
                   existing.TickSize != updated.TickSize ||
                   existing.Currency != updated.Currency ||
                   existing.BaseCurrency != updated.BaseCurrency;
        }
    }
}

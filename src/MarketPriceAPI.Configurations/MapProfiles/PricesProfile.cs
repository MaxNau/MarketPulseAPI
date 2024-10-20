using AutoMapper;
using MarketPulseAPI.Fintacharts.Models.Bars;
using MarketPulseAPI.Fintacharts.Models.Dtos;
using MarketPulseAPI.Fintacharts.Models.Queries;

namespace MarketPriceAPI.Configurations.MapProfiles
{
    public class PricesProfile : Profile
    {
        public PricesProfile()
        {
            CreateMap<HistoricalPriceQuery, BarsDateRangeQuery>()
                .ConstructUsing(s => new BarsDateRangeQuery(s.AssetId.ToString(), s.Provider, s.Interval, s.Periodicity.ToString(), s.StartDate, s.EndDate));
            CreateMap<Bar, HistoricalPrice>();
        }
    }
}

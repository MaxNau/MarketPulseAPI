using System.Text.Json.Serialization;

namespace MarketPulseAPI.Fintacharts.Models
{
    public class Paging
    {
        [JsonConstructor]
        public Paging(int page, int pages, int items)
        {
            Page = page;
            Pages = pages;
            Items = items;
        }

        public int Page { get; }
        public int Pages { get; }
        public int Items { get; }
    }
}

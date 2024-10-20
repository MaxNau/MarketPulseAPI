using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MarketPulseAPI.Fintacharts.Models.Dtos
{
    public class HistoricalPriceQuery
    {
        [Required]
        public Guid AssetId { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        [Range(1, 60)]
        public int Interval { get; set; }

        [Required]
        [EnumDataType(typeof(Periodicity))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Periodicity Periodicity { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Periodicity
    {
        Minute,
        Hour,
        Month,
        Year
    }
}

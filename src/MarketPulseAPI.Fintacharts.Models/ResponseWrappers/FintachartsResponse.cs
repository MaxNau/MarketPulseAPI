using System.Text.Json.Serialization;

namespace MarketPulseAPI.Fintacharts.Models.ResponseWrappers
{
    public class FintachartsResponse<T>
    {
        [JsonConstructor]
        public FintachartsResponse(IReadOnlyCollection<T> data)
        {
            Data = data;
        }

        public IReadOnlyCollection<T> Data { get; }
    }
}

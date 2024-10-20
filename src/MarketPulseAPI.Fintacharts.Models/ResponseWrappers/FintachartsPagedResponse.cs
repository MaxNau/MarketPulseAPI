using System.Text.Json.Serialization;

namespace MarketPulseAPI.Fintacharts.Models.ResponseWrappers
{
    public class FintachartsPagedResponse<T>
    {
        [JsonConstructor]
        public FintachartsPagedResponse(Paging paging, IReadOnlyCollection<T> data)
        {
            Paging = paging;
            Data = data;
        }

        public Paging Paging { get; }
        public IReadOnlyCollection<T> Data { get; }
    }
}

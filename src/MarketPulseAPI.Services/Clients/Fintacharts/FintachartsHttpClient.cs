using MarketPulseAPI.Fintacharts.Models;
using MarketPulseAPI.Fintacharts.Models.Bars;
using MarketPulseAPI.Fintacharts.Models.Instruments;
using MarketPulseAPI.Fintacharts.Models.Queries;
using MarketPulseAPI.Fintacharts.Models.ResponseWrappers;
using MarketPulseAPI.Interfaces.Clients.Fintacharts;
using MarketPulseAPI.Services.TokenStores;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MarketPulseAPI.Services.Clients.Fintacharts
{
    public class FintachartsHttpClient : IFintachartsHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStore<FintachartsToken> _tokenStore;
        private readonly JsonSerializerOptions _options = new()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        public FintachartsHttpClient(HttpClient httpClient, ITokenStore<FintachartsToken> tokenStore)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;
        }

        public async Task<FintachartsPagedResponse<Instrument>?> GetInstrumentsAsync(InstrumentsQuery? instrumentsQuery = null, CancellationToken cancellationToken = default)
        {
            SetToken();

            using var response = await _httpClient.GetAsync($"api/instruments/v1/instruments{instrumentsQuery?.ToQueryString()}", cancellationToken);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<FintachartsPagedResponse<Instrument>>(responseStream, _options, cancellationToken);
        }

        public async Task<FintachartsResponse<Bar>?> GetBarsAsync(BarsDateRangeQuery barsDateRangeQuery, CancellationToken cancellationToken = default)
        {
            SetToken();

            using var response = await _httpClient.GetAsync($"api/bars/v1/bars/date-range{barsDateRangeQuery?.ToQueryString()}", cancellationToken);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return await JsonSerializer.DeserializeAsync<FintachartsResponse<Bar>>(responseStream, _options, cancellationToken);
        }

        private void SetToken()
        {
            var token = (_tokenStore.Token?.Token) ?? throw new InvalidOperationException("Token is missing");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}

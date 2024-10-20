using MarketPulseAPI.Fintacharts.Models.Dtos;
using MarketPulseAPI.Interfaces.Services.Mid;
using Microsoft.AspNetCore.Mvc;

namespace MarketPulseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsPricesController : ControllerBase
    {
        private readonly IAssetsPricesService _assetsPricesService;
        public AssetsPricesController(IAssetsPricesService assetsPricesService)
        {
            _assetsPricesService = assetsPricesService;
        }

        [HttpGet("Historical")]
        public async Task<IActionResult> GetAsync([FromQuery] HistoricalPriceQuery historicalPriceQuery, CancellationToken cancellationToken)
        {
            var result = await _assetsPricesService.GetHistoricalPricesAsync(historicalPriceQuery, cancellationToken);
            return Ok(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("ws/realtime")]
        public async Task Get(Guid assetId, CancellationToken cancellationToken = default)
        {
            if (assetId == Guid.Empty)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await HttpContext.Response.WriteAsync("Asset ID is required.");
                return;
            }

            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _assetsPricesService.GetRealTimePricesAsync(webSocket, assetId, cancellationToken);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}

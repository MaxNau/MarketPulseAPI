using MarketPulseAPI.Interfaces.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MarketPulseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketAssetsController : ControllerBase
    {
        private readonly IAssetsRepository _assetsRepository;
        public MarketAssetsController(IAssetsRepository assetsRepository)
        {
            _assetsRepository = assetsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            return Ok(await _assetsRepository.GetAllAsync());
        }
    }
}

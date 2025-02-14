using Microsoft.AspNetCore.Mvc;
using Profile.BLL.Interfaces.Services;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("global-statistics")]
    public class GlobalStatisticsController : ControllerBase
    {
        private readonly IGlobalStatisticsServices _globalStatisticsServices;
        private readonly ILogger<GlobalStatisticsController> _logger;

        public GlobalStatisticsController(IGlobalStatisticsServices globalStatisticsServices, ILogger<GlobalStatisticsController> logger)
        {
            _globalStatisticsServices = globalStatisticsServices;
            _logger = logger;
        }

        [HttpGet("moves/most-used")]
        public async Task<IActionResult> GetMostUsedMove(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetMostUsedMove] Fetching the most used move statistics.");

            var result = await _globalStatisticsServices.GetMostUsedMoveAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("moves/winrate")]
        public async Task<IActionResult> GetMovesWinRate(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetMovesWinRate] Fetching moves win rate statistics.");

            var result = await _globalStatisticsServices.GetMoveWinRateAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("moves/usage")]
        public async Task<IActionResult> GetMovesUsageStatistics(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetMovesUsageStatistics] Fetching moves usage statistics.");

            var result = await _globalStatisticsServices.GetMoveUsageStatisticsAsync(cancellationToken);

            return Ok(result);
        }
    }
}

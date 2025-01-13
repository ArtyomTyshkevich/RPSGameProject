using Microsoft.AspNetCore.Mvc;
using Profile.BLL.Interfaces.Services;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("GlobalStatistics")]
    public class GlobalStatisticsController : ControllerBase
    {
        private readonly IGlobalStatisticsServices _globalStatisticsServices;
        public GlobalStatisticsController(IGlobalStatisticsServices globalStatisticsService)
        {
            _globalStatisticsServices = globalStatisticsService;
        }

        [HttpGet("moves/most-used")]
        public async Task<ActionResult> GetMostUsedMove(CancellationToken cancellationToken)
        {
            return Ok(await _globalStatisticsServices.GetMostUsedMoveAsync(cancellationToken));
        }

        [HttpGet("moves/win-rate")]
        public async Task<ActionResult> GetMovesWinRatePlayers(CancellationToken cancellationToken)
        {
            return Ok(await _globalStatisticsServices.GetMoveWinRateAsync(cancellationToken));
        }

        [HttpGet("moves/usage")]
        public async Task<ActionResult> GetMovesUsageStatistics(CancellationToken cancellationToken)
        {
            return Ok(await _globalStatisticsServices.GetMoveUsageStatisticsAsync(cancellationToken));
        }

    }
}

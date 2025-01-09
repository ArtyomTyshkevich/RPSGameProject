using Microsoft.AspNetCore.Mvc;
using Profile.BLL.Interfaces.Services;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("UserStatistics")]
    public class UserStatisticsController : ControllerBase
    {
        private readonly IUserStatisticsService _userStatisticsService;

        public UserStatisticsController(IUserStatisticsService userStatisticsService)
        {
            _userStatisticsService = userStatisticsService;
        }

        [HttpGet("move/statistics")]
        public async Task<ActionResult> GetMoveStatisticsAsync(Guid playerId, CancellationToken cancellationToken)
        {
         return Ok(await  _userStatisticsService.GetMoveStatisticsAsync(playerId, cancellationToken));
        }

        [HttpGet("win/count")]
        public async Task<ActionResult> GetWinsConuntAsync(Guid playerId, CancellationToken cancellationToken)
        {
            return Ok(await _userStatisticsService.GetWinsCountAsync(playerId, cancellationToken));
        }
        [HttpGet("lose/count")]
        public async Task<ActionResult> GetLosesCountAsync(Guid playerId, CancellationToken cancellationToken)
        {
            return Ok(await _userStatisticsService.GetLossesCountAsync(playerId, cancellationToken));
        }

        [HttpGet("win-rate")]
        public async Task<ActionResult> GetWinRateAsync(Guid playerId, CancellationToken cancellationToken)
        {
            return Ok(await _userStatisticsService.GetWinRateAsync(playerId, cancellationToken));
        }

    }
}

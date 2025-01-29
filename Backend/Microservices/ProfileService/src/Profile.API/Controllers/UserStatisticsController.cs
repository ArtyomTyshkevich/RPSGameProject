using Microsoft.AspNetCore.Mvc;
using Profile.BLL.Interfaces.Services;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("user-statistics")]
    public class UserStatisticsController : ControllerBase
    {
        private readonly IUserStatisticsService _userStatisticsService;
        private readonly ILogger<UserStatisticsController> _logger;

        public UserStatisticsController(IUserStatisticsService userStatisticsService, ILogger<UserStatisticsController> logger)
        {
            _userStatisticsService = userStatisticsService;
            _logger = logger;
        }

        [HttpGet("move/statistics")]
        public async Task<ActionResult> GetMoveStatisticsAsync(Guid playerId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetMoveStatisticsAsync] Fetching move statistics for player with ID: {PlayerId}.", playerId);

            var result = await _userStatisticsService.GetMoveStatisticsAsync(playerId, cancellationToken);

            _logger.LogInformation("[GetMoveStatisticsAsync] Successfully fetched move statistics for player with ID: {PlayerId}.", playerId);

            return Ok(result);
        }

        [HttpGet("win/count")]
        public async Task<ActionResult> GetWinsCountAsync(Guid playerId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetWinsCountAsync] Fetching win count for player with ID: {PlayerId}.", playerId);

            var result = await _userStatisticsService.GetWinsCountAsync(playerId, cancellationToken);

            _logger.LogInformation("[GetWinsCountAsync] Successfully fetched win count for player with ID: {PlayerId}.", playerId);

            return Ok(result);
        }

        [HttpGet("lose/count")]
        public async Task<ActionResult> GetLossesCountAsync(Guid playerId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetLossesCountAsync] Fetching losses count for player with ID: {PlayerId}.", playerId);

            var result = await _userStatisticsService.GetLossesCountAsync(playerId, cancellationToken);

            _logger.LogInformation("[GetLossesCountAsync] Successfully fetched losses count for player with ID: {PlayerId}.", playerId);

            return Ok(result);
        }

        [HttpGet("winrate")]
        public async Task<ActionResult> GetWinRateAsync(Guid playerId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetWinRateAsync] Fetching win rate for player with ID: {PlayerId}.", playerId);

            var result = await _userStatisticsService.GetWinRateAsync(playerId, cancellationToken);

            _logger.LogInformation("[GetWinRateAsync] Successfully fetched win rate for player with ID: {PlayerId}.", playerId);

            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Profile.BLL.Interfaces.Services;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("users/{Id}/statistics")]
    public class UserStatisticsController : ControllerBase
    {
        private readonly IUserStatisticsService _userStatisticsService;
        private readonly ILogger<UserStatisticsController> _logger;

        public UserStatisticsController(IUserStatisticsService userStatisticsService, ILogger<UserStatisticsController> logger)
        {
            _userStatisticsService = userStatisticsService;
            _logger = logger;
        }

        [HttpGet("moves")]
        public async Task<ActionResult> GetMoveStatisticsAsync(Guid Id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching move statistics for player {PlayerId}.", Id);

            var games = await _userStatisticsService.GetAllUserGames(Id, cancellationToken);
            var result = await _userStatisticsService.GetMoveStatisticsAsync(games, Id, cancellationToken);

            _logger.LogInformation("Successfully fetched move statistics for player {PlayerId}.", Id);

            return Ok(result);
        }

        [HttpGet("wins/count")]
        public async Task<ActionResult> GetWinsCountAsync(Guid Id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching win count for player {PlayerId}.", Id);

            var games = await _userStatisticsService.GetAllUserGames(Id, cancellationToken);
            var result = await _userStatisticsService.GetWinsCountAsync(games, Id, cancellationToken);

            _logger.LogInformation("Successfully fetched win count for player {PlayerId}.", Id);

            return Ok(result);
        }

        [HttpGet("losses/count")]
        public async Task<ActionResult> GetLossesCountAsync(Guid Id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching losses count for player {PlayerId}.", Id);
            var games = await _userStatisticsService.GetAllUserGames(Id, cancellationToken);
            var result = await _userStatisticsService.GetLossesCountAsync(games, Id, cancellationToken);

            _logger.LogInformation("Successfully fetched losses count for player {PlayerId}.", Id);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetUserStatisticsAsync(Guid Id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching statistics for player {PlayerId}.", Id);
            var games = await _userStatisticsService.GetAllUserGames(Id, cancellationToken);
            var result = await _userStatisticsService.CreateUserWithStatistics(games, Id, cancellationToken);

            _logger.LogInformation("Successfully fetched statistics for player {PlayerId}.", Id);

            return Ok(result);
        }

        [HttpGet("winrate")]
        public async Task<ActionResult> GetWinRateAsync(Guid Id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching win rate for player {PlayerId}.", Id);
            var games = await _userStatisticsService.GetAllUserGames(Id, cancellationToken);
            var result = await _userStatisticsService.GetWinRateAsync(games, Id, cancellationToken);

            _logger.LogInformation("Successfully fetched win rate for player {PlayerId}.", Id);

            return Ok(result);
        }
    }
}

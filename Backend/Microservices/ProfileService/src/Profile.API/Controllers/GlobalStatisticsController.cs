using Microsoft.AspNetCore.Mvc;
using Profile.BLL.DTOs;
using Profile.BLL.Interfaces.Services;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class GlobalStatisticsController : ControllerBase
    {
        private readonly IGlobalStatisticsServices _globalStatisticsServices;
        private readonly ILogger<GlobalStatisticsController> _logger;
        private readonly IGameService _gameService;

        public GlobalStatisticsController(IGlobalStatisticsServices globalStatisticsServices, IGameService gameService, ILogger<GlobalStatisticsController> logger)
        {
            _globalStatisticsServices = globalStatisticsServices;
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet("moves/most-used")]
        public async Task<IActionResult> GetMostUsedMove(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetMostUsedMove] Fetching the most used move statistics.");
            var games = await _gameService.GetAllGamesAsync(cancellationToken);
            var result = await _globalStatisticsServices.GetMostUsedMoveAsync(games, cancellationToken);

            return Ok(result);
        }

        [HttpGet("moves/winrate")]
        public async Task<IActionResult> GetMovesWinRate(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetMovesWinRate] Fetching moves win rate statistics.");

            var games = await _gameService.GetAllGamesAsync(cancellationToken);
            var result = await _globalStatisticsServices.GetMoveWinRateAsync(games, cancellationToken);

            return Ok(result);
        }

        [HttpGet("moves/usage")]
        public async Task<IActionResult> GetMovesUsageStatistics(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetMovesUsageStatistics] Fetching moves usage statistics.");

            var games = await _gameService.GetAllGamesAsync(cancellationToken);
            var result = await _globalStatisticsServices.GetMoveUsageStatisticsAsync(games, cancellationToken);

            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Profile.BLL.DTOs;
using Profile.BLL.Interfaces.Services;
using Profile.DAL.Entities.Mongo;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GameController> _logger;

        public GameController(IGameService gameService, ILogger<GameController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Game>>> GetAllGames(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetAllGames] Retrieving all games");
            var games = await _gameService.GetAllGamesAsync(cancellationToken);
            return Ok(games);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> PostGame([FromBody] GameDTO gameDTO, CancellationToken cancellationToken)
        {
            if (gameDTO == null)
            {
                _logger.LogWarning("[PostGame] Received null GameDTO");
                return BadRequest("Game data is required.");
            }

            _logger.LogInformation("[PostGame] Adding new game: {GameName}", gameDTO.Name);
            await _gameService.AddGameAsync(gameDTO, cancellationToken);
            return Ok();
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Game>> GetGameById(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("[GetGameById] Received null or empty id");
                return BadRequest("Game ID is required.");
            }

            _logger.LogInformation("[GetGameById] Retrieving game with ID: {GameId}", id);
            var game = await _gameService.GetGameByIdAsync(id, cancellationToken);
            if (game == null)
            {
                _logger.LogWarning("[GetGameById] Game with ID {GameId} not found", id);
                return NotFound($"Game with ID {id} not found.");
            }

            return Ok(game);
        }

        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> DeleteGame(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("[DeleteGame] Received null or empty id");
                return BadRequest("Game ID is required.");
            }

            _logger.LogInformation("[DeleteGame] Deleting game with ID: {GameId}", id);
            await _gameService.DeleteGameAsync(id, cancellationToken);
            return NoContent();
        }
    }
}

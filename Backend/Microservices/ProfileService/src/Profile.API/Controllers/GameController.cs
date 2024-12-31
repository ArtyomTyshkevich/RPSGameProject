using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Profile.BLL.Interfaces.Services;
using Profile.DAL.Entities.Mongo;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateGame([FromBody] Game game, CancellationToken cancellationToken)
        {
            await _gameService.AddGameAsync(game, cancellationToken);
            return Ok(game);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Game>>> GetAllGames(CancellationToken cancellationToken)
        {
            var games = await _gameService.GetAllGamesAsync(cancellationToken);
            return Ok(games);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Game>> GetGameById(string id, CancellationToken cancellationToken)
        {
            var game = await _gameService.GetGameByIdAsync(id, cancellationToken);
            if (game == null)
            {
                return NotFound(); 
            }
            return Ok(game);
        }

        [HttpPut("UpdateById/{id}")]
        public async Task<ActionResult> UpdateGame(string id, [FromBody] Game game, CancellationToken cancellationToken)
        {
            game.Id = id;
            await _gameService.UpdateGameAsync(game, cancellationToken);
            return NoContent();
        }

        [HttpDelete("DeleteById/{id}")]
        public async Task<ActionResult> DeleteGame(string id, CancellationToken cancellationToken)
        {
            await _gameService.DeleteGameAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
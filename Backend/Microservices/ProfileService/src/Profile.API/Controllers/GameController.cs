using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Profile.BLL.DTOs;
using Profile.BLL.Interfaces.Services;

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

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<GameDTO>>> GetAllGames(CancellationToken cancellationToken)
        {
            var games = await _gameService.GetAllGamesAsync(cancellationToken);
            return Ok(games);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<GameDTO>> GetGameById(string id, CancellationToken cancellationToken)
        {
            var game = await _gameService.GetGameByIdAsync(id, cancellationToken);
            return Ok(game);
        }

        [HttpDelete("DeleteById/{id}")]
        public async Task<ActionResult> DeleteGame(string id, CancellationToken cancellationToken)
        {
            await _gameService.DeleteGameAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
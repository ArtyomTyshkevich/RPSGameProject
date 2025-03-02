﻿using Microsoft.AspNetCore.Mvc;
using Profile.BLL.DTOs;
using Profile.BLL.Interfaces.Services;
using Profile.DAL.Entities.Mongo;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("games")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GamesController> _logger;

        public GamesController(IGameService gameService, ILogger<GamesController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Game>>> GetAllGames(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetAllGames] Retrieving all games");

            var games = await _gameService.GetAllGamesAsync(cancellationToken);

            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGameById(string id, CancellationToken cancellationToken)
        {

            _logger.LogInformation("[GetGameById] Retrieving game with ID: {GameId}", id);

            var game = await _gameService.GetGameByIdAsync(id, cancellationToken);

            return Ok(game);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(string id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[DeleteGame] Deleting game with ID: {GameId}", id);

            await _gameService.DeleteGameAsync(id, cancellationToken);

            return NoContent();
        }
    }
}

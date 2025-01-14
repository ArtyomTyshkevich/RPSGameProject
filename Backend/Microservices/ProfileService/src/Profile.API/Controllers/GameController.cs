﻿using AutoMapper;
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

        public GameController(IGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Game>>> GetAllGames(CancellationToken cancellationToken)
        {
            var games = await _gameService.GetAllGamesAsync(cancellationToken);
            return Ok(games);
        }
        [HttpPost("Add")]
        public async Task<ActionResult<List<Game>>> PostGames(GameDTO gameDTO, CancellationToken cancellationToken)
        {
             await _gameService.AddGameAsync(gameDTO, cancellationToken);
            return Ok();
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Game>> GetGameById(string id, CancellationToken cancellationToken)
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
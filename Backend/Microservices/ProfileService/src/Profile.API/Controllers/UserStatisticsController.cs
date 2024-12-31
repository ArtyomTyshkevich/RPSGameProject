using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Profile.BLL.Interfaces.Repositories;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("UserStatistics")]
    public class UserStatisticsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserStatisticsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("getMoveStatisticsById")]
        public async Task<ActionResult> GetPlayerMoveStatistics(string playerId)
        {
            return Ok();
        }

        [HttpGet("getWinRateById")]
        public async Task<ActionResult> GetPlayerWins(string playerId)
        {
            return Ok();
        }
        [HttpGet("getLossRateById")]
        public async Task<ActionResult> GetPlayerLosses(string playerId)
        {
            return Ok();
        }

        [HttpGet("statistics/player/{playerId}/move/win-rate")]
        public async Task<ActionResult> GetPlayerMoveWinRate(string playerId)
        {
            return Ok();
        }

    }
}

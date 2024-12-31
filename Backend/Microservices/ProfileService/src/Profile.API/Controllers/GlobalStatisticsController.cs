using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Profile.BLL.Interfaces.Repositories;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("GlobalStatistics")]
    public class GlobalStatisticsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GlobalStatisticsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetMostUsedMove")]
        public async Task<ActionResult> GetMostUsedMove()
        {
            return Ok();
        }

        [HttpGet("statistics/move/win-rate")]
        public async Task<ActionResult> GetMoveWinRateForAllPlayers()
        {
            return Ok();
        }

        [HttpGet("statistics/move-usage")]
        public async Task<ActionResult> GetMoveUsageStatistics()
        {
            return Ok();
        }

    }
}

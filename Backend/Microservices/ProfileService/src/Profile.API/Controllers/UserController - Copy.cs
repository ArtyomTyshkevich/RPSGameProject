using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Profile.BLL.DTOs;
using Profile.BLL.Interfaces.Repositories;
using Profile.DAL.Entities;

namespace Game.WebAPI.Controllers
{
    [ApiController]
    [Route("user")]
    public class StatisticsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StatisticsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllSortedByRating")]
        public async Task<ActionResult<List<UserDTO>>> GetAllSortedByRating(CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users
                .GetAllAsync(cancellationToken);

            var sortedUsers = _mapper.ProjectTo<UserDTO>(users.AsQueryable())
                                     .OrderByDescending(u => u.Rating);

            return Ok(sortedUsers.ToList());
        }

    }
}

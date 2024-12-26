using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Profile.BLL.DTOs;
using Profile.BLL.Interfaces.Repositories;
using Profile.DAL.Entities;

namespace Game.WebAPI.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<UserDTO>>> GetAll(CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);
            var userDTO = _mapper.ProjectTo<UserDTO>(users.AsQueryable());
            return Ok(userDTO.ToList());
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<UserDTO>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }

        [HttpPut("UpdateById")]
        public async Task<ActionResult> Update(UserDTO userDTO, CancellationToken cancellationToken)
        {
            await _unitOfWork.Users.UpdateAsync(_mapper.Map<User>(userDTO));
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}

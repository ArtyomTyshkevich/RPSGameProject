using Microsoft.AspNetCore.Mvc;
using Profile.BLL.DTOs;
using Profile.BLL.Interfaces.Services;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<UserDTO>>> GetAll(CancellationToken cancellationToken)
        {
            var usersDTO = await _userService.GetAllAsync(cancellationToken);
            return Ok(usersDTO);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<UserDTO>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var userDTO = await _userService.GetByIdAsync(id, cancellationToken);
            return Ok(userDTO);
        }

        [HttpPut("UpdateById")]
        public async Task<ActionResult> Update(UserDTO userDTO, CancellationToken cancellationToken)
        {
            await _userService.UpdateAsync(userDTO, cancellationToken);
            return NoContent();
        }

        [HttpDelete("DeleteById")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
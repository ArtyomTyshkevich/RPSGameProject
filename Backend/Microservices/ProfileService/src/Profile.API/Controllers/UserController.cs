﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.BLL.DTOs;
using Profile.BLL.Interfaces.Services;

namespace Profile.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<UserDTO>>> GetAll(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetAll] Fetching all users.");

            var usersDTO = await _userService.GetAllAsync(cancellationToken);

            _logger.LogInformation("[GetAll] Successfully fetched {UserCount} users.", usersDTO.Count);

            return Ok(usersDTO);
        }

        [HttpGet("sorted-by-rating")]
        [Authorize]
        public async Task<ActionResult<List<UserDTO>>> GetUsersSortedByRatingWithPagination(int page, int pageSize, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetAll] Fetching all users.");

            var usersDTO = await _userService.GetUsersSortedByRatingWithPagination(page, pageSize, cancellationToken);

            _logger.LogInformation("[GetAll] Successfully fetched {UserCount} users.", usersDTO.Count);

            return Ok(usersDTO);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDTO>> GetById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetById] Fetching user with ID: {UserId}.", id);

            var userDTO = await _userService.GetByIdAsync(id, cancellationToken);
            if (userDTO == null)
            {
                _logger.LogWarning("[GetById] User with ID: {UserId} not found.", id);

                return NotFound();
            }

            _logger.LogInformation("[GetById] Successfully fetched user with ID: {UserId}.", id);
            return Ok(userDTO);
        }

        [HttpPut]
        public async Task<ActionResult> Update(UserDTO userDTO, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Update] Updating user with ID: {UserId}.", userDTO.Id);

            await _userService.UpdateAsync(userDTO, cancellationToken);

            _logger.LogInformation("[Update] Successfully updated user with ID: {UserId}.", userDTO.Id);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Delete] Deleting user with ID: {UserId}.", id);

            await _userService.DeleteAsync(id, cancellationToken);

            _logger.LogInformation("[Delete] Successfully deleted user with ID: {UserId}.", id);

            return NoContent();
        }

        [HttpGet("total-count")]
        [Authorize]
        public async Task<ActionResult<int>> GetTotalUserCount(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetTotalUserCount] Fetching total user count.");

            var userCount = await _userService.GetTotalUserCountAsync(cancellationToken);

            _logger.LogInformation("[GetTotalUserCount] Successfully fetched total user count: {UserCount}.", userCount);

            return Ok(userCount);
        }
    }
}

using Game.Application.DTOs;
using Game.Application.Interfaces.Services;
using Game.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Game.WebAPI.Controllers
{
    [ApiController]
    [Route("rooms")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly ILogger<RoomController> _logger;

        public RoomController(IRoomService roomService, ILogger<RoomController> logger)
        {
            _roomService = roomService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoomDTO>>> GetAll(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to fetch all rooms.");
            var roomsDTO = await _roomService.GetAllRoomsAsync(cancellationToken);
            _logger.LogInformation("Successfully fetched {Count} rooms.", roomsDTO.Count);
            return Ok(roomsDTO);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoomDTO>> GetById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching room with ID: {RoomId}.", id);
            var room = await _roomService.GetRoomByIdAsync(id, cancellationToken);
            if (room == null)
            {
                _logger.LogWarning("Room with ID: {RoomId} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully fetched room with ID: {RoomId}.", id);
            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWithRounds([FromBody] RoomDTO roomDTO, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to create a room with 3 rounds.");
            await _roomService.CreateRoomWithRoundsAsync(roomDTO, 3, cancellationToken);
            _logger.LogInformation("Successfully created a room.");
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to delete room with ID: {RoomId}.", id);
            await _roomService.DeleteRoomByIdAsync(id, cancellationToken);
            _logger.LogInformation("Successfully deleted room with ID: {RoomId}.", id);
            return NoContent();
        }

        [HttpDelete("inactive")]
        public async Task<IActionResult> DeleteInactiveRooms(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to delete inactive rooms.");
            await _roomService.DeleteInactiveRoomsAsync(cancellationToken);
            _logger.LogInformation("Successfully deleted inactive rooms.");
            return NoContent();
        }

        [HttpPatch("{roomId:guid}/status")]
        public async Task<IActionResult> UpdateRoomStatus(Guid roomId, [FromQuery] RoomStatuses newStatus, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to update status of room with ID: {RoomId} to {NewStatus}.", roomId, newStatus);
            await _roomService.UpdateRoomStatusAsync(roomId, newStatus, cancellationToken);
            _logger.LogInformation("Successfully updated status of room with ID: {RoomId} to {NewStatus}.", roomId, newStatus);
            return Ok();
        }
    }
}

using AutoMapper;
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

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoomDTO>>> GetAll(CancellationToken cancellationToken)
        {
            var roomsDTO = await _roomService.GetAllRoomsAsync(cancellationToken);
            return Ok(roomsDTO);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoomDTO>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var room = await _roomService.GetRoomByIdAsync(id, cancellationToken);
            if (room == null) return NotFound();

            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWithRounds([FromBody] RoomDTO roomDTO, CancellationToken cancellationToken)
        {
            await _roomService.CreateRoomWithRoundsAsync(roomDTO, 3, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _roomService.DeleteRoomByIdAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpDelete("inactive")]
        public async Task<IActionResult> DeleteInactiveRooms(CancellationToken cancellationToken)
        {
            await _roomService.DeleteInactiveRoomsAsync(cancellationToken);
            return NoContent();
        }

        [HttpPatch("{roomId:guid}/status")]
        public async Task<IActionResult> UpdateRoomStatus(Guid roomId, [FromQuery] RoomStatuses newStatus, CancellationToken cancellationToken)
        {
            await _roomService.UpdateRoomStatusAsync(roomId, newStatus, cancellationToken);
            return Ok();
        }
    }
}

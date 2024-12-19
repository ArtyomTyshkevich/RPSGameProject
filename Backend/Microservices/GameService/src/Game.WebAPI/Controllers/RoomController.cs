using AutoMapper;
using Game.Application.DTOs;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Entities;
using Game.Domain.Enums;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Game.WebAPI.Controllers
{
    [ApiController]
    [Route("rooms")]
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRoomService _roomService;

        public RoomController(IUnitOfWork unitOfWork, IMapper mapper, IRoomService roomService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roomService = roomService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<RoomDTO>>> GetAll(CancellationToken cancellationToken)
        {
            var rooms = await _unitOfWork.Rooms.GetAllAsync(cancellationToken);
            var roomsDTO = _mapper.ProjectTo<RoomDTO>(rooms.AsQueryable());

            return Ok(roomsDTO.ToList());
        }

        [HttpPost("GetById")]
        public async Task<ActionResult<RoomDTO>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var room = _mapper.Map<RoomDTO>(await _unitOfWork.Rooms.GetByIdAsync(id, cancellationToken));
            if (room == null) return NotFound();

            return Ok(room);
        }

        [HttpPost("CreateWithRounds")]
        public async Task<IActionResult> CreateWithRounds(RoomDTO roomDTO, CancellationToken cancellationToken)
        {
            await _roomService.CreateRoomWithRounds(_mapper.Map<Room>(roomDTO), 3, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _unitOfWork.Rooms.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        [HttpDelete("deleteInactive")]
        public async Task<IActionResult> DeleteInactiveRooms(CancellationToken cancellationToken)
        {
            await _unitOfWork.Rooms.DeleteInactiveRoomsAsync(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        [HttpPatch("{roomId}/status")]
        public async Task<IActionResult> UpdateRoomStatus(Guid roomId, RoomStatuses newStatus, CancellationToken cancellationToken)
        {
            await _unitOfWork.Rooms.UpdateRoomStatusAsync(roomId, newStatus, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok();
        }
        [HttpGet("active/count")]
        public async Task<IActionResult> GetTotalActiveRoomsCount(CancellationToken cancellationToken)
        {
            var count = await _unitOfWork.Rooms.GetTotalActiveRoomsCountAsync(cancellationToken);
            return Ok(count);
        }
    }
}

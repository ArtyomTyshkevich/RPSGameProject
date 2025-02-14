using AutoMapper;
using Game.Application.DTOs;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Entities;
using Game.Domain.Enums;

namespace Game.Data.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateRoomWithRoundsAsync(RoomDTO roomDTO, int numRounds, CancellationToken cancellationToken)
        {
            var room = _mapper.Map<Room>(roomDTO);
            await _unitOfWork.Rooms.CreateAsync(room);
            for (int i = 0; i < numRounds; i++)
            {
                room.Rounds.Add(new Round { });
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Guid?> AddUserToRoomAsync(User user, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Rooms.GetAvailableRoomAsync(RoomTypes.Default);

            if (room != null)
            {
                _unitOfWork.Rooms.Attach(room);

                if (room.FirstPlayer == null)
                {
                    room.FirstPlayer = user;
                }
                else if (room.SecondPlayer == null)
                {
                    room.SecondPlayer = user;
                    room.Status = RoomStatuses.InGame;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return room.Id;
            }

            return null;
        }

        public async Task<List<RoomDTO>> GetAllRoomsAsync(CancellationToken cancellationToken)
        {
                var rooms = await _unitOfWork.Rooms.GetAllAsync(cancellationToken);
            return _mapper.ProjectTo<RoomDTO>(rooms.AsQueryable()).ToList();
        }

        public async Task<RoomDTO> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id, cancellationToken);
            var roomDTO = _mapper.Map<RoomDTO>(room);
            return roomDTO;
        }

        public async Task DeleteRoomByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            await _unitOfWork.Rooms.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteInactiveRoomsAsync(CancellationToken cancellationToken)
        {
            await _unitOfWork.Rooms.DeleteInactiveRoomsAsync(cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRoomStatusAsync(Guid roomId, RoomStatuses newStatus, CancellationToken cancellationToken)
        {
            await _unitOfWork.Rooms.UpdateRoomStatusAsync(roomId, newStatus, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

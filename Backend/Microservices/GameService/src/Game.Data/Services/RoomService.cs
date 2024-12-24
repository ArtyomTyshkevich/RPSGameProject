using AutoMapper;
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

        public async Task CreateRoomWithRounds(Room room, int numRounds, CancellationToken cancellationToken)
        {
            await _unitOfWork.Rooms.CreateAsync(room);
            for (int i = 0; i < numRounds; i++)
            {
                room.Rounds.Add(new Round { });
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> AddUserToRoom(User user, CancellationToken cancellationToken)
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
                return true;
            }

            return false;
        }
    }
}

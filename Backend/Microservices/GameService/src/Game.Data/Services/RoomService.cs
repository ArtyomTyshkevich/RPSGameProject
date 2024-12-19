
using AutoMapper;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Entities;

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
    }
}

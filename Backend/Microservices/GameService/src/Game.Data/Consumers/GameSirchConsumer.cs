using Game.Application.DTOs;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Domain.Enums;
using MassTransit;

namespace Chat.Data.Consumers
{
    public class GameSirchConsumer : IConsumer<UserDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GameSirchConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<UserDTO> context)
        {
            while (true)
            {
                var user = await _unitOfWork.Users.GetByIdNoTrackingAsync(context.Message.Id);
                if (user.Status != UserStatuses.InSearch)
                    break;

                var room = await _unitOfWork.Rooms
                    .GetAvailableRoomAsync(RoomTypes.Defoult);

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

                    await _unitOfWork.SaveChangesAsync();
                    break;
                }
                else
                {
                    await Task.Delay(10000);
                }
            }
        }
    }
}

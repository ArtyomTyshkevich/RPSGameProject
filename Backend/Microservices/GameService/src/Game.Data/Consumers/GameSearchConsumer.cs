using Game.Application.DTOs;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Enums;
using MassTransit;

namespace Game.Data.Consumers
{
    public class GameSearchConsumer : IConsumer<UserDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomService _roomService;

        public GameSearchConsumer(IUnitOfWork unitOfWork, IRoomService roomService)
        {
            _unitOfWork = unitOfWork;
            _roomService = roomService;
        }

        public async Task Consume(ConsumeContext<UserDTO> context)
        {
            var cancellationToken = context.CancellationToken;
            var userId = context.Message.Id;

            while (true)
            {
                var user = await _unitOfWork.Users.GetByIdNoTrackingAsync(userId, cancellationToken);
                if (user.Status != UserStatuses.InSearch)
                {
                    break;
                }

                var roomId = await _roomService.AddUserToRoomAsync(user, cancellationToken);
                if (roomId != null)
                {
                    await context.RespondAsync(new RoomResponse { RoomId = roomId.Value });
                    break;
                }
                else
                {
                    await Task.Delay(10000, cancellationToken);
                }
            }
        }
    }
}

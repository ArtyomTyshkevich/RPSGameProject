using Game.Application.DTOs;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Enums;
using MassTransit;

namespace Chat.Data.Consumers
{
    public class GameSirchConsumer : IConsumer<UserDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomService _roomService;

        public GameSirchConsumer(IUnitOfWork unitOfWork, IRoomService roomService)
        {
            _unitOfWork = unitOfWork;
            _roomService = roomService;
        }

        public async Task Consume(ConsumeContext<UserDTO> context)
        {
            var cancellationToken = context.CancellationToken;

            while (true)
            {
                var user = await _unitOfWork.Users.GetByIdNoTrackingAsync(context.Message.Id, cancellationToken);
                if (user.Status != UserStatuses.InSearch)
                    break;
                if (await _roomService.AddUserToRoom(user, cancellationToken))
                {
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

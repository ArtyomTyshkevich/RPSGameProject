using AutoMapper;
using Broker.Events;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using MassTransit;

namespace Game.Data.Consumers
{
    public class UserDeleteConsumer : IConsumer<UserDeletedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UserDeleteConsumer(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<UserDeletedEvent> context)
        {
            var cancellationToken = context.CancellationToken;
            await _unitOfWork.Users.DeleteAsync(context.Message.Id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

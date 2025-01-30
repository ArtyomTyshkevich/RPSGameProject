using AutoMapper;
using Broker.Events;
using Library.Application.Interfaces;
using MassTransit;

namespace Auth.BLL.Consumers
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
            await _unitOfWork.Users.(context.Message.Id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

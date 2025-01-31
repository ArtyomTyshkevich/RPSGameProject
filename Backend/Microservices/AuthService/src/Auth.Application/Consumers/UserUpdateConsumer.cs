using Auth.DAL.Entities;
using AutoMapper;
using Broker.Events;
using Library.Application.Interfaces;
using MassTransit;

namespace Auth.BLL.Consumers
{
    public class UserUpdateConsumer : IConsumer<UserUpdatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UserUpdateConsumer(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            var cancellationToken = context.CancellationToken;
            var user = _mapper.Map<User>(context.Message);
            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

using AutoMapper;
using Broker.Events;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Domain.Entities;
using MassTransit;

namespace Profile.DAL.Events
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
            await _unitOfWork.Users.UpdateAsync(_mapper.Map<User>(context.Message), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

using AutoMapper;
using Game.Application.DTOs;
using Game.Application.Interfaces.Buses;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Enums;
using MassTransit;

namespace Game.Data.Services
{
    public class SearchService : ISearchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISearchRabbitMqBus _bus;
        private readonly IMapper _mapper;

        public SearchService(IUnitOfWork unitOfWork, ISearchRabbitMqBus bus, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _bus = bus;
        }

        public async Task StartSearchGame(Guid userID, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userID);
            await _unitOfWork.Users.UpdateUserStatusAsync(userID, UserStatuses.InSearch);
            await _unitOfWork.SaveChangesAsync();
            var userDTO = _mapper.Map<UserDTO>(user);
            await _bus.Publish(userDTO, cancellationToken);
        }

        public async Task StopSearchGame(Guid userID, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userID);
            await _unitOfWork.Users.UpdateUserStatusAsync(userID, UserStatuses.Active);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
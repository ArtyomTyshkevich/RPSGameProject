using AutoMapper;
using Game.Application.DTOs;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Enums;
using MassTransit;

namespace Game.Data.Services
{
    public class SearchService : ISearchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public SearchService(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }

        public async Task StartSearchGame(Guid userID, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userID);
            await _unitOfWork.Users.UpdateUserStatusAsync(userID, UserStatuses.InSearch);
            await _unitOfWork.SaveChangesAsync();
            var userDTO = _mapper.Map<UserDTO>(user);
            await _publishEndpoint.Publish(userDTO, cancellationToken);
        }
        public async Task StopSearchGame(Guid userID, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userID);
            await _unitOfWork.Users.UpdateUserStatusAsync(userID, UserStatuses.Active);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
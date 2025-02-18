using AutoMapper;
using Broker.Events;
using MassTransit;
using Profile.BLL.DTOs;
using Profile.BLL.Interfaces.Repositories;
using Profile.BLL.Interfaces.Services;
using Profile.DAL.Entities;

namespace Profile.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);
            return _mapper.ProjectTo<UserDTO>(users.AsQueryable()).ToList();
        }

        public async Task<UserDTO> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task UpdateAsync(UserDTO userDTO, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(userDTO);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _publishEndpoint.Publish(
                _mapper.Map<UserUpdatedEvent>(user),
                context =>
                {
                    context.SetRoutingKey("user.updated");
                },
                cancellationToken);

        }

        public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken)
        {
            await _unitOfWork.Users.DeleteAsync(userId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _publishEndpoint.Publish(new UserDeletedEvent {Id = userId }, cancellationToken);
        }

        public async Task UpdateRating(Guid userId, int rating, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            user.Rating = rating;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task<List<UserDTO>> GetUsersSortedByRatingWithPagination(int page, int pageSize, CancellationToken cancellationToken = default)
        {
           var users = await _unitOfWork.Users.GetUsersSortedByRatingWithPagination(page, pageSize, cancellationToken);
           return _mapper.ProjectTo<UserDTO>(users.AsQueryable()).ToList();
        }
        public async Task<int> GetTotalUserCountAsync(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Users.GetTotalUserCountAsync(cancellationToken);
        }

    }
}
using AutoMapper;
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

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAllAsync(cancellationToken);
            return _mapper.ProjectTo<UserDTO>(users.AsQueryable()).ToList();
        }

        public async Task<UserDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task UpdateAsync(UserDTO userDTO, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(userDTO);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
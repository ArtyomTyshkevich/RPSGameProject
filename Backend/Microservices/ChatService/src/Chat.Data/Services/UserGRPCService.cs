using AutoMapper;
using Chat.Application.Interfaces;
using Chat.Domain.Entities;
using Grpc.Core;
using UserGrpcService;
using static UserGrpcService.UserService;

namespace Chat.Data.Services
{
    public class UserGRPCService : UserServiceBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserGRPCService(IUserRepository userRepository,IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public override async Task<SaveUserResponse> SendUserData(UserRequest request, ServerCallContext context)
        {
            try
            {
                await _unitOfWork.Users.AddAsync(_mapper.Map<User>(request));
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new SaveUserResponse
                {
                    Success = false,
                    Message = ex.Message
                };

            }
            return new SaveUserResponse
            {
                Success = true,
                Message = "Ok"
            };
        }
    }
}
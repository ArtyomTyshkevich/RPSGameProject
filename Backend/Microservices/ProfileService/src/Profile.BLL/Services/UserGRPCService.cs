using AutoMapper;
using Grpc.Core;
using Profile.BLL.Interfaces.Repositories;
using Profile.DAL.Entities;
using UserGrpcService;
using static UserGrpcService.ProfileServiceGRPC;

namespace Profile.BLL.Services
{
    public class UserGRPCService : ProfileServiceGRPCBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserGRPCService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public override async Task<SaveUserResponse> SaveUser(UserRequest request, ServerCallContext context)
        {
            try
            {
                await _unitOfWork.Users.CreateAsync(_mapper.Map<User>(request));
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
using MediatR;
using AutoMapper;
using Auth.BLL.Commands.GrpcCommands;
using UserGrpcService;
using Library.Application.Interfaces;

namespace Auth.BLL.Handlers.CommandHandlers.GrpcCommands
{
    public class SendUserDataCommandHandler : IRequestHandler<SendUserDataCommand, Unit>
    {
        //private readonly ProfileServiceGRPC.ProfileServiceGRPCClient _profileClient;
        private readonly GameServiceGRPC.GameServiceGRPCClient _gameClient;
        private readonly ChatServiceGRPC.ChatServiceGRPCClient _chatClient;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SendUserDataCommandHandler(
            //ProfileServiceGRPC.ProfileServiceGRPCClient profileClient,
            GameServiceGRPC.GameServiceGRPCClient gameClient,
            ChatServiceGRPC.ChatServiceGRPCClient chatClient,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            //_profileClient = profileClient;
            _gameClient = gameClient;
            _chatClient = chatClient;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(SendUserDataCommand request, CancellationToken cancellationToken)
        {
            var userData = _mapper.Map<UserRequest>(await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken));

            //var profileTask = _profileClient.SaveUserAsync(userData, cancellationToken: cancellationToken).ResponseAsync;
            var gameTask = _gameClient.SaveUserAsync(userData, cancellationToken: cancellationToken).ResponseAsync;
            var chatTask = _chatClient.SaveUserAsync(userData, cancellationToken: cancellationToken).ResponseAsync;

            await Task.WhenAll(/*profileTask,*/ gameTask, chatTask);

            return Unit.Value;
        }
    }
}

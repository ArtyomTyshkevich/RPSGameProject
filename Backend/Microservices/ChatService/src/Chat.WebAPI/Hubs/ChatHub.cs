using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Chat.WebAPI.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _cache;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IUnitOfWork unitOfWork, IDistributedCache cache, ICacheService cacheService, IMapper mapper, ILogger<ChatHub> logger)
        {
            _mapper = mapper;
            _cache = cache;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task JoinChatAsync(Guid id)
        {
            _logger.LogInformation("[JoinChatAsync] User {UserId} attempting to join chat.", id);

            var userDTO = _mapper.Map<UserDTO>(await _unitOfWork.Users.GetByIdAsync(id));
            if (userDTO == null)
            {
                _logger.LogWarning("[JoinChatAsync] User {UserId} not found.", id);
                return;
            }

            var connection = new UserConnection { UserDTO = userDTO, ChatRoom = "MainRoom" };
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
            await _cacheService.ConnectionAsync(Context.ConnectionId, connection);

            _logger.LogInformation("[JoinChatAsync] User {UserId} joined chat room {ChatRoom}.", id, connection.ChatRoom);
        }

        public async Task SendMessageAsync(string messageContext)
        {
            _logger.LogInformation("[SendMessageAsync] Message received from connection {ConnectionId}.", Context.ConnectionId);

            var connection = await _cacheService.GetConnectionAsync(Context.ConnectionId);
            if (connection == null)
            {
                _logger.LogWarning("[SendMessageAsync] No connection found for {ConnectionId}.", Context.ConnectionId);
                return;
            }

            var message = Message.Create(connection.UserDTO, messageContext);
            await Clients
                .Group(connection.ChatRoom)
                .ReceiveMessage(message);

            await _unitOfWork.Messages.SaveMessageAsync(message);

            _logger.LogInformation("[SendMessageAsync] Message sent to chat room {ChatRoom} from user {UserId}.", connection.ChatRoom, connection.UserDTO.Id);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("[OnDisconnectedAsync] Connection {ConnectionId} disconnected.", Context.ConnectionId);

            var connection = await _cacheService.GetConnectionAsync(Context.ConnectionId);
            if (connection != null)
            {
                await _cache.RemoveAsync(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);

                _logger.LogInformation("[OnDisconnectedAsync] Connection {ConnectionId} removed from chat room {ChatRoom}.", Context.ConnectionId, connection.ChatRoom);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}

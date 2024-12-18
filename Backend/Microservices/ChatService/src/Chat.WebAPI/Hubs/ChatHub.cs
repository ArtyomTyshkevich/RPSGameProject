using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace Chat.WebAPI.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _cache;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;

        public ChatHub(IUnitOfWork unitOfWork, IDistributedCache cache, ICacheService cacheService, IMapper mapper)
        {
            _mapper = mapper;
            _cache = cache;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
        }

        public async Task JoinChatAsync(Guid id)
        {
            var userDTO = _mapper.Map<UserDTO>(_unitOfWork.Users.GetByIdAsync(id));
            var connection = new UserConnection {UserDTO = userDTO, ChatRoom ="MainRoom"};
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
            await _cacheService.ConnectionAsync(Context.ConnectionId, connection);
        }

        public async Task SendMessageAsync(string messageContext)
        {
            var connection = await _cacheService.GetConnectionAsync(Context.ConnectionId);
            if (connection != null)
            {
                var message = Message.Create(connection.UserDTO,messageContext);
                await Clients
                    .Group(connection.ChatRoom)
                    .ReceiveMessage(message);
                await _unitOfWork.Messages.SaveMessageAsync(message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = await _cacheService.GetConnectionAsync(Context.ConnectionId);
            if (connection != null)
            {
                await _cache.RemoveAsync(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);

            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
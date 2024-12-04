using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace Chat.API.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _cache;
        private readonly ICacheService _cacheService;

        public ChatHub(IUnitOfWork unitOfWork, IDistributedCache cache, ICacheService cacheService)
        {
            _cache = cache;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
        }

        public async Task JoinChat(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
            await _cacheService.CachingConnection(Context.ConnectionId, connection);
        }

        public async Task SendMessage(string messageContext)
        {

            var connection = await _cacheService.GetConnectionFromCache(Context.ConnectionId);
            if (connection != null)
            {
                var message = _unitOfWork.Messages.Create(connection.UserDTO,messageContext);
                await Clients
                    .Group(connection.ChatRoom)
                    .ReceiveMessage(connection.UserDTO, message);

            }

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = await _cacheService.GetConnectionFromCache(Context.ConnectionId);
            if (connection != null)
            {
                await _cache.RemoveAsync(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);

            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
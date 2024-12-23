using AutoMapper;
using Game.Application.DTOs;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace Game.WebAPI.Hubs
{
    public class GameHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoundService _roundService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly ICacheService _cacheService;
        public GameHub( IUnitOfWork unitOfWork, IRoundService roundService, IMapper mapper, IDistributedCache cache, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _roundService = roundService;
            _mapper = mapper;
            _cache = cache;
            _cacheService = cacheService;
        }

        public async Task SendMove(PlayerMoves move)
        {
            var connection = await _cacheService.GetConnectionFromCache(Context.ConnectionId);

            if (connection != null)
            {
                var roomId = Guid.Parse(connection.GameRoomId);
                var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
                using (var cts = new CancellationTokenSource())
                {
                    var cancellationToken = cts.Token;
                    await _roundService.ProcessRound(room, connection.UserId, move, cancellationToken);

                    await Clients.Group(roomId.ToString()).SendAsync("ReceiveMove", room);
                }
            }
        }

        public async Task JoinChat(Guid userId, Guid roomId)
        {
            var connection = new UserConnection { UserId = userId, GameRoomId = roomId.ToString()};
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.GameRoomId);
            await _cacheService.CachingConnection(Context.ConnectionId, connection);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = await _cacheService.GetConnectionFromCache(Context.ConnectionId);
            if (connection != null)
            {
                await _cache.RemoveAsync(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.GameRoomId);

            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
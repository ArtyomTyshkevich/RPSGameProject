using AutoMapper;
using Game.Application.DTOs;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;

namespace Game.WebAPI.Hubs
{
    public class GameHub : Hub
    {
        private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _roomLocks = new();
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoundService _roundService;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly ICacheService _cacheService;
        private readonly ILogger<GameHub> _logger;

        public GameHub(IUnitOfWork unitOfWork, IRoundService roundService, IMapper mapper, IDistributedCache cache, ICacheService cacheService, ILogger<GameHub> logger)
        {
            _unitOfWork = unitOfWork;
            _roundService = roundService;
            _mapper = mapper;
            _cache = cache;
            _cacheService = cacheService;
            _logger = logger;
        }
        public async Task SendMove(PlayerMoves move)
        {
            var connectionId = Context.ConnectionId;

            _logger.LogInformation("[SendMove] Started. ConnectionId: {ConnectionId}, Move: {Move}", connectionId, move);

            var connection = await _cacheService.GetConnection(connectionId);
            if (connection != null)
            {
                var roomId = Guid.Parse(connection.GameRoomId);
                _logger.LogInformation("[SendMove] Processing move for RoomId: {RoomId}, UserId: {UserId}", roomId, connection.UserId);

                // Получаем или создаем семафор для комнаты
                var roomLock = _roomLocks.GetOrAdd(roomId, _ => new SemaphoreSlim(1, 1));

                try
                {
                    // Ожидаем захвата семафора
                    await roomLock.WaitAsync();

                    var room = await _unitOfWork.Rooms.GetByIdAsNoTrakingAsync(roomId);

                    var cancellationTokenSource = new CancellationTokenSource();
                    var cancellationToken = cancellationTokenSource.Token;

                    var message = await _roundService.ProcessRound(room, connection.UserId, move, cancellationToken);

                    await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", message);

                    _logger.LogInformation("[SendMove] Move processed successfully for RoomId: {RoomId}", roomId);
                }
                finally
                {
                    // Освобождаем семафор
                    roomLock.Release();
                }
            }
        }

        public async Task JoinChat(Guid userId, Guid roomId)
        {
            _logger.LogInformation("[JoinChat] Started. UserId: {UserId}, RoomId: {RoomId}", userId, roomId);

            var connection = new UserConnection { UserId = userId, GameRoomId = roomId.ToString() };
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.GameRoomId);
            await _cacheService.SetConnection(Context.ConnectionId, connection);
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            var allPlayersInRoom = room.FirstPlayer != null && room.SecondPlayer != null;
            if (allPlayersInRoom)
            {
                await Clients.Group(roomId.ToString()).SendAsync("AllPlayersInRoom");
            }    
            _logger.LogInformation("[JoinChat] User {UserId} joined Room {RoomId}. ConnectionId: {ConnectionId}", userId, roomId, Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("[OnDisconnectedAsync] Started. ConnectionId: {ConnectionId}", Context.ConnectionId);

            var connection = await _cacheService.GetConnection(Context.ConnectionId);
            if (connection != null)
            {
                await _cache.RemoveAsync(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.GameRoomId);

                _logger.LogInformation("[OnDisconnectedAsync] User disconnected. ConnectionId: {ConnectionId}, RoomId: {RoomId}", Context.ConnectionId, connection.GameRoomId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}

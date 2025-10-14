using Game.Application.DTOs;
using Game.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace Chat.Data.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly IDatabase _redis;

    public CacheService(IDistributedCache cache, IConnectionMultiplexer redis)
    {
        _cache = cache;
        _redis = redis.GetDatabase();
    }
    public async Task SetConnection(string connectionId, UserConnection connection)
    {
        var stringConnection = JsonSerializer.Serialize(connection);
        await _cache.SetStringAsync(connectionId, stringConnection);

        var roomKey = $"room:{connection.GameRoomId}";
        await _redis.SetAddAsync(roomKey, connection.UserId.ToString());
    }

    public async Task<UserConnection?> GetConnection(string connectionId)
    {
        var stringConnection = await _cache.GetStringAsync(connectionId);
        return stringConnection == null ? null : JsonSerializer.Deserialize<UserConnection>(stringConnection);
    }

    public async Task DeleteConnection(string connectionId)
    {
        var stringConnection = await _cache.GetStringAsync(connectionId);
        if (stringConnection == null) return;

        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);
        await _cache.RemoveAsync(connectionId);

        if (connection != null)
        {
            var roomKey = $"room:{connection.GameRoomId}";
            await _redis.SetRemoveAsync(roomKey, connection.UserId.ToString());
        }
    }

    public async Task<int> GetPlayerCountInRoom(Guid roomId)
    {
        var roomKey = $"room:{roomId}";
        var count = await _redis.SetLengthAsync(roomKey);
        return (int)count;
    }
}

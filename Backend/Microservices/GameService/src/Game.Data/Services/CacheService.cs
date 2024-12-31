using Game.Application.DTOs;
using Game.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Chat.Data.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SetConnection(string connectionId, UserConnection connection)
    {
        var stringConnection = JsonSerializer.Serialize(connection);
        await _cache.SetStringAsync(connectionId, stringConnection);
    }

    public async Task<UserConnection?> GetConnection(string connectionId)
    {
        var stringConnection = await _cache.GetAsync(connectionId);
        return JsonSerializer.Deserialize<UserConnection>(stringConnection);
    }

    public async Task DeleteConnection(string connectionId)
    {
        var stringConnection = await _cache.GetAsync(connectionId);
        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

        if (connection != null)
        {
            await _cache.RemoveAsync(connectionId);
        }
    }
}

using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Chat.Data.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService( IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task ConnectionAsync(string connectionId, UserConnection connection)
    {
        var stringConnection = JsonSerializer.Serialize(connection);
        await _cache.SetStringAsync(connectionId, stringConnection);
    }

    public async Task<UserConnection?> GetConnectionAsync(string connectionId)
    {
        var stringConnection = await _cache.GetAsync(connectionId);
        return JsonSerializer.Deserialize<UserConnection>(stringConnection);
    }

    public async Task DeleteConnectionAsync(string connectionId)
    {
        var stringConnection = await _cache.GetAsync(connectionId);
        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

        if (connection != null)
        {
            await _cache.RemoveAsync(connectionId);
        }
    }
}

using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Chat.Data.Services;

public class CacheService : ICacheService
{
    private readonly IUserRepository _userRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IDistributedCache _cache;

    public CacheService(IUserRepository userRepository, IMessageRepository messageRepository, IDistributedCache cache)
    {
        _userRepository = userRepository;
        _messageRepository = messageRepository;
        _cache = cache;
    }

    public async Task CachingConnectionAsync(string connectionId, UserConnection connection)
    {
        var stringConnection = JsonSerializer.Serialize(connection);
        await _cache.SetStringAsync(connectionId, stringConnection);
    }

    public async Task<UserConnection?> GetConnectionFromCacheAsync(string connectionId)
    {
        var stringConnection = await _cache.GetAsync(connectionId);
        return JsonSerializer.Deserialize<UserConnection>(stringConnection);
    }

    public async Task DeleteConnectionFromCacheAsync(string connectionId)
    {
        var stringConnection = await _cache.GetAsync(connectionId);
        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

        if (connection != null)
        {
            await _cache.RemoveAsync(connectionId);
        }
    }
}

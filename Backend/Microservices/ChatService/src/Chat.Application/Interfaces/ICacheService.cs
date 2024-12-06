using Chat.Application.DTOs;

namespace Chat.Application.Interfaces
{
    public interface ICacheService
    {
        Task CachingConnectionAsync(string connectionId, UserConnection connection);
        Task DeleteConnectionFromCacheAsync(string connectionId);
        Task<UserConnection?> GetConnectionFromCacheAsync(string connectionId);
    }
}
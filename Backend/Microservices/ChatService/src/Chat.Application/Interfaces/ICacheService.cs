using Chat.Application.DTOs;

namespace Chat.Application.Interfaces
{
    public interface ICacheService
    {
        Task CachingConnection(string connectionId, UserConnection connection);
        Task DeleteConnectionFromCache(string connectionId);
        Task<UserConnection?> GetConnectionFromCache(string connectionId);
    }
}
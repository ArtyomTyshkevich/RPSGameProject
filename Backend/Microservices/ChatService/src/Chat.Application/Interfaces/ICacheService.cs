using Chat.Application.DTOs;

namespace Chat.Application.Interfaces
{
    public interface ICacheService
    {
        Task ConnectionAsync(string connectionId, UserConnection connection);
        Task DeleteConnectionAsync(string connectionId);
        Task<UserConnection?> GetConnectionAsync(string connectionId);
    }
}
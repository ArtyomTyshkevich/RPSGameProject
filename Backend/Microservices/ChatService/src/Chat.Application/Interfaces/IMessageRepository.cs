using Chat.Application.DTOs;
using Chat.Domain.Entities;

namespace Chat.Application.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesAsync(string chatRoom);
        Task SaveMessageAsync(Message message);
    }
}
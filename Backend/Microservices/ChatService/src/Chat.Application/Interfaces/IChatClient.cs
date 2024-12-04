
using Chat.Application.DTOs;
using Chat.Domain.Entities;

namespace Chat.Application.Interfaces
{
    public interface IChatClient
    {
        public Task ReceiveMessage(UserDTO userDTO, Message message);
    }
}

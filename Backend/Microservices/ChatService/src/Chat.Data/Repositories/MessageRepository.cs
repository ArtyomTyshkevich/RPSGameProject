using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Data.Context;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Chat.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<Message> _messages;

        public MessageRepository(MongoDbContext context)
        {
            _messages = context.Messages;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(string chatRoom)
        {
            return await _messages.Find(m => m.Content.Contains(chatRoom)).ToListAsync();
        }

        public async Task SaveMessageAsync(Message message)
        {
            await _messages.InsertOneAsync(message);
        }
    }
}

using Chat.Application.Interfaces;
using Chat.Data.Context;
using Chat.Domain.Entities;
using MongoDB.Driver;

namespace Chat.Data.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatDbContext _chatDbContext;
        private readonly MongoDbContext _mongoDbContext;

        public UnitOfWork(ChatDbContext chatDbContext, MongoDbContext mongoDbContext)
        {
            _chatDbContext = chatDbContext;
            _mongoDbContext = mongoDbContext;
            Messages = new MessageRepository(_mongoDbContext);
            Users = new UserRepository(_chatDbContext);
        }

        public IUserRepository Users { get; private set; }
        public IMessageRepository Messages { get; private set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _chatDbContext.SaveChangesAsync(cancellationToken);

        public void Dispose() => _chatDbContext.Dispose();
    }
}
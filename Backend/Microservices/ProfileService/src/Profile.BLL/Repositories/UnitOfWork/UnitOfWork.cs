﻿using Microsoft.Extensions.Options;
using Profile.BLL.Configuretion;
using Profile.BLL.Interfaces.Repositories;
using Profile.DAL.Data;

namespace Profile.BLL.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProfileDbContext _gameDbContext;
        IOptions<MongoDbConfiguration> mongoDbConfiguration;
        public UnitOfWork(ProfileDbContext gameDbContext, IOptions<MongoDbConfiguration> mongoDbConfiguration)
        {
            _gameDbContext = gameDbContext;
            Users = new UserRepository(_gameDbContext);
            Games = new GameRepository(mongoDbConfiguration);
        }

        public IUserRepository Users { get; private set; }
        public IGameRepository Games { get; private set; }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _gameDbContext.SaveChangesAsync(cancellationToken);
        public void Dispose() => _gameDbContext.Dispose();
    }
}
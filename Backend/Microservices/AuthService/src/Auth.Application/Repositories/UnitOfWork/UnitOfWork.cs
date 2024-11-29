using Auth.BLL.Interfaces;
using Auth.DAL.Data;
using Library.Application.Interfaces;
using Library.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Auth.BLL.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _userDbContext;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public UnitOfWork(AuthDbContext libraryDbContext, IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _userDbContext = libraryDbContext;
            _configuration = configuration;
            Users = new UserRepository(_userDbContext);
        }

        public IUserRepository Users { get; private set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _userDbContext.SaveChangesAsync(cancellationToken);

        public void Dispose() => _userDbContext.Dispose();
    }
}
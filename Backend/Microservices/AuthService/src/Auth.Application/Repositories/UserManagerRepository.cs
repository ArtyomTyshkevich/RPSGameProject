//using Auth.BLL.Interfaces;
//using Auth.DAL.Data;
//using Auth.DAL.Entities;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace Library.Data.Repositories
//{
//    public class UserManagerRepository : IUserRepository
//    {
//        private readonly AuthDbContext _authDbContext;

//        public UserRepository(AuthDbContext authDbContext)
//        {
//            _authDbContext = authDbContext;
//        }

//        public async Task Create(User user, CancellationToken cancellationToken)
//        {
//            await _authDbContext.Users.AddAsync(user, cancellationToken);
//            await _authDbContext.SaveChangesAsync(cancellationToken);
//        }

//        public async Task Delete(User user, CancellationToken cancellationToken)
//        {
//            _authDbContext.Users.Remove(user);
//            await _authDbContext.SaveChangesAsync(cancellationToken);
//        }

//        public async Task<List<User>> Get(CancellationToken cancellationToken)
//        {
//            return await _authDbContext.Users
//                                            .Include(b => b.UserName)
//                                            .ToListAsync(cancellationToken);
//        }

//        public async Task<User?> GetById(long userId, CancellationToken cancellationToken)
//        {
//            return await _authDbContext.Users
//                                 .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
//        }
//        public async Task<List<long>> GetRolesId(User user, CancellationToken cancellationToken)
//        {
//            var roleIds = await _authDbContext.UserRoles
//                .Where(ur => ur.UserId == user.Id)
//                .Select(ur => ur.RoleId)
//                .ToListAsync(cancellationToken);

//            return roleIds;
//        }

//        public async Task<List<IdentityRole<long>>> GetRoles(User entity, CancellationToken cancellationToken)
//        {
//            var roleIds = await GetRolesId(entity, cancellationToken);

//            var roles = await _authDbContext.Roles
//                .Where(role => roleIds.Contains(role.Id))
//                .ToListAsync(cancellationToken);

//            return roles;
//        }

//        public async Task<List<User>> GetWithPagination(int pageNum, int pageSize, CancellationToken cancellationToken)
//        {
//            return await _authDbContext.Users
//                                           .Skip((pageNum - 1) * pageSize)
//                                           .Take(pageSize)
//                                           .ToListAsync(cancellationToken);
//        }

//        public async Task Update(User user, CancellationToken cancellationToken)
//        {
//            _authDbContext.Users.Update(user);
//            await _authDbContext.SaveChangesAsync(cancellationToken);
//        }
//        public async Task<User> GetByMail(string mail, CancellationToken cancellationToken)
//        {
//            return await _authDbContext.Users
//                .FirstAsync(u => u.Email == mail, cancellationToken);
//        }
//    }
//}

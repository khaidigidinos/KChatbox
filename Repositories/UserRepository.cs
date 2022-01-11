using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalRApi.DatabaseContexts;
using SignalRApi.Entities.Room;
using SignalRApi.Entities.User;
using SignalRApi.Enums;
using SignalRApi.Ultilities;

namespace SignalRApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDatabaseContext _dbContext;
        private readonly IRoomDatabaseContext _roomDatabaseContext;

        public UserRepository(IUserDatabaseContext dbContext, IRoomDatabaseContext roomDatabaseContext)
        {
            _dbContext = dbContext;
            _roomDatabaseContext = roomDatabaseContext;
        }

        public async Task<UserEntity> GetUserByEmail(string email, bool isIncludeRoles, CancellationToken cancellationToken = default)
        {
            var buildingQuery = _dbContext.Users.AsNoTracking().Where(user => user.Email == email);
            if (isIncludeRoles)
            {
                buildingQuery.Include(user => user.Roles);
            }
            return await buildingQuery.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<UserEntity>> GetUsersByName(string name, CancellationToken cancellationToken = default)
        {
            return await (from user in _dbContext.Users.Include(u => u.Roles)
                        where user.Name.Contains(name)
                        select user).ToListAsync(cancellationToken);
        }

        public async Task<UserEntity> Login(string username, string useremail, string userpassword, CancellationToken cancelationToken = default)
        {
            var userInfo = await _dbContext.Users
                .AsNoTracking()
                .Where(user => user.Name == username &&
                               user.Email == useremail &&
                               user.Password == PasswordHashUltility.HashPassowrd(userpassword))
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(cancelationToken);
            return userInfo;
        }

        public async Task<UserEntity> Register(string username, string useremail, string userpassword, CancellationToken cancellationToken = default)
        {
            var user = new User();
            await _roomDatabaseContext.Users.InsertOneAsync(user);
            var newUser = new UserEntity()
            {
                Name = username,
                Email = useremail,
                Password = PasswordHashUltility.HashPassowrd(userpassword),
                Roles = new List<RoleEntity>() { await _dbContext.Roles.Where(role => role.Name == "User").FirstOrDefaultAsync() },
                Status = Status.Active,
                Id = user.Id
            };

            await _dbContext.Users.AddAsync(newUser, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return newUser;
        }

        public async Task UpdateUserFirebaseToken(string userId, string newToken, string oldToken, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FindAsync(userId, cancellationToken);

            var firebaseTokens = user.FirebaseTokens;
            var listTokens = firebaseTokens
                .Split(";")
                .ToList();
            listTokens.Remove(oldToken);
            listTokens.Add(newToken);
            user.FirebaseTokens = string.Join(',', listTokens.ToArray());

            await _dbContext.SaveChangesAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignalRApi.Entities.User;

namespace SignalRApi.Repositories
{
    public interface IUserRepository
    {
        public Task<UserEntity> Login(string username, string useremail, string userpassword, CancellationToken cancelationToken = default);
        public Task<UserEntity> Register(string username, string useremail, string userpassword, CancellationToken cancellationToken = default);
        public Task<UserEntity> GetUserByEmail(string email, bool isIncludeRoles, CancellationToken cancellationToken = default);
        public Task<List<UserEntity>> GetUsersByName(string name, CancellationToken token = default);
        public Task UpdateUserFirebaseToken(string userId, string newToken, string oldToken, CancellationToken cancellationToken = default);
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalRApi.Entities.User;

namespace SignalRApi.DatabaseContexts
{
    public interface IUserDatabaseContext
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
    }
}

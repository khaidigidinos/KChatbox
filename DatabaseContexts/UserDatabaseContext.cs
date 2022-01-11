using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalRApi.Entities.User;
using SignalRApi.Ultilities;

namespace SignalRApi.DatabaseContexts
{
    public class UserDatabaseContext : DbContext, IUserDatabaseContext
    {
        public UserDatabaseContext(DbContextOptions<UserDatabaseContext> options)
        : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entity in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreatedAt = DateTimeUltility.UnixTimeStamp;
                        entity.Entity.UpdatedAt = DateTimeUltility.UnixTimeStamp;
                        break;
                    case EntityState.Modified:
                        entity.Entity.UpdatedAt = DateTimeUltility.UnixTimeStamp;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
    }
}

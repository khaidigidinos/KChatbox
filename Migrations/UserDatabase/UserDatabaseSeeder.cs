using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalRApi.DatabaseContexts;
using SignalRApi.Entities.User;
using SignalRApi.Enums;
using SignalRApi.Migrations.RoomDatabase;
using SignalRApi.Ultilities;

namespace SignalRApi.Migrations.UserDatabase
{
    public static class UserDatabaseSeeder
    {
        public static async Task InitRoleData(IUserDatabaseContext dbContext)
        {
            var count = await dbContext.Roles.CountAsync();

            if (count <= 0)
            {
                /* Initial data. Add more if necessary */
                var rolesList = new RoleEntity[]
                {
                    new RoleEntity() { Name = "User" },
                    new RoleEntity() { Name = "Admin" }
                };

                await dbContext.Roles.AddRangeAsync(rolesList);
                await dbContext.SaveChangesAsync(default);
            }
        }

        public static async Task InitUserData(IUserDatabaseContext dbContext, List<string> ids)
        {
            var count = await dbContext.Users.CountAsync();

            if (count <= 0)
            {
                /* Initial data. Add more if necessary */
                var usersList = new UserEntity[]
                {
                    new UserEntity()
                    {
                        Name = "khai29012001",
                        Email = "khai29012001@gmail.com",
                        Password =  PasswordHashUltility.HashPassowrd("khaidigidinos"),
                        Status = Status.Active,
                        Roles = await dbContext.Roles.ToListAsync(),
                        Id = ids[0]
                    },
                    new UserEntity()
                    {
                        Name = "vuathamdu",
                        Email = "vuathamdu@gmail.com",
                        Password =  PasswordHashUltility.HashPassowrd("khaidigidinos"),
                        Status = Status.Active,
                        Roles = await dbContext.Roles.Where(role => role.Name != "Admin").ToListAsync(),
                        Id = ids[1]
                    }
                };

                await dbContext.Users.AddRangeAsync(usersList);
                await dbContext.SaveChangesAsync(default);
            }
        }
    }
}

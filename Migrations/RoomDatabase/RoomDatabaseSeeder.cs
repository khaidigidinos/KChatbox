using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SignalRApi.DatabaseContexts;
using SignalRApi.Entities.Room;
using SignalRApi.Migrations.UserDatabase;

namespace SignalRApi.Migrations.RoomDatabase
{
    public static class RoomDatabaseSeeder
    {
        public static async Task InitUserData(IRoomDatabaseContext dbContext, IUserDatabaseContext userDatabaseContext)
        {
            if ((await dbContext.Users.EstimatedDocumentCountAsync()) > 0)
            {
                return;
            }

            var usersList = new User[]
            {
                new User(),
                new User()
            };

            await dbContext.Users.InsertManyAsync(usersList);
            await UserDatabaseSeeder.InitUserData(userDatabaseContext, usersList.Select(user => user.Id).ToList());
        }
    }
}

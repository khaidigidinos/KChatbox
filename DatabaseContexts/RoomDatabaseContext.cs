using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using SignalRApi.Entities.Room;

namespace SignalRApi.DatabaseContexts
{
    public class RoomDatabaseContext : IRoomDatabaseContext
    {
        private readonly IMongoCollection<Room> _rooms;
        private readonly IMongoCollection<User> _users;

        public RoomDatabaseContext(IConfiguration configuration)
        {
            var mongoDbClient = new MongoClient(configuration.GetValue<string>("ConnectionStrings:MessageContext:ConnectionString"));
            var dbContext = mongoDbClient.GetDatabase(configuration.GetValue<string>("ConnectionStrings:MessageContext:DatabaseName"));

            _rooms = dbContext.GetCollection<Room>(configuration.GetValue<string>("ConnectionStrings:MessageContext:RoomCollectionName"));
            _users = dbContext.GetCollection<User>(configuration.GetValue<string>("ConnectionStrings:MessageContext:UserCollectionName"));
        }

        public IMongoCollection<Room> Rooms { get => _rooms; }
        public IMongoCollection<User> Users { get => _users; }
    }
}

using System;
using MongoDB.Driver;
using SignalRApi.Entities.Room;

namespace SignalRApi.DatabaseContexts
{
    public interface IRoomDatabaseContext
    {
        public IMongoCollection<Room> Rooms { get; }
        public IMongoCollection<User> Users { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using SignalRApi.DatabaseContexts;
using SignalRApi.Entities.Room;
using SignalRApi.Exceptions;
using SignalRApi.Ultilities;

namespace SignalRApi.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IRoomDatabaseContext _roomDatabaseContext;

        public RoomRepository(IRoomDatabaseContext roomDatabaseContext)
        {
            _roomDatabaseContext = roomDatabaseContext;
        }

        public async Task<Room> CreateNewRoom(string name, List<string> userIds, CancellationToken cancelationToken = default)
        {
            var newRoom = new Room() { Name = name, Users = userIds };
            await _roomDatabaseContext.Rooms.InsertOneAsync(newRoom, new InsertOneOptions(), cancelationToken);
            return newRoom;
        }

        public async Task AddNewRoomToUsers(List<string> userIds, string roomId, CancellationToken cancellationToken = default)
        {
            var users = await (await _roomDatabaseContext.Users.FindAsync(user => userIds.Contains(user.Id), new FindOptions<User, User>(), cancellationToken)).ToListAsync();
            foreach (var user in users)
            {
                if (user.Rooms is null) {
                    user.Rooms = new List<string>();
                }
                user.Rooms.Add(roomId);
            }

            await Task.WhenAll(users.Select(async user => await _roomDatabaseContext.Users.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: cancellationToken)).ToList());
        }

        public async Task<List<Room>> GetRoomsForUser(string userId, CancellationToken cancellationToken = default)
        {
            var user = await (await _roomDatabaseContext.Users.FindAsync(user => user.Id == userId, new FindOptions<User, User>(), cancellationToken)).FirstOrDefaultAsync();
            if (user is null)
            {
                throw new NotFoundException(nameof(User));
            }

            var projection = Builders<Room>.Projection.Exclude(room => room.Messages);
            var sort = Builders<Room>.Sort.Descending(room => room.UpdatedAt);

            if (user.Rooms is null)
            {
                return new List<Room>();
            }

            var rooms = await (await _roomDatabaseContext.Rooms
                .FindAsync(room => user.Rooms.Contains(room.Id), new FindOptions<Room, Room>() { Projection = projection, Sort = sort }, cancellationToken))
                .ToListAsync();

            return rooms;
        }

        public async Task NewMessageForRoom(string roomId, long sentAt, string content, string senderId, CancellationToken cancellationToken = default)
        {
            var room = await (await _roomDatabaseContext.Rooms.FindAsync(room => room.Id == roomId, cancellationToken: cancellationToken)).FirstOrDefaultAsync(cancellationToken);
            if (room.Messages is null)
            {
                room.Messages = new List<Room.Message>();
            }

            var newMessage = new Room.Message()
            {
                SenderId = senderId,
                SentAt = sentAt,
                Content = content
            };

            room.Messages.Add(newMessage);

            room.Messages.Sort(new Room.MessageComparer());
            room.UpdatedAt = DateTimeUltility.UnixTimeStamp;

            await _roomDatabaseContext.Rooms.ReplaceOneAsync(room => room.Id == roomId, room, cancellationToken: cancellationToken);
        }

        public async Task<Room> GetRoomWithMessages(string roomId, CancellationToken cancellationToken = default)
        {
            return await (await _roomDatabaseContext.Rooms.FindAsync(room => room.Id == roomId, cancellationToken: cancellationToken)).FirstOrDefaultAsync(cancellationToken);
        }
    }
}

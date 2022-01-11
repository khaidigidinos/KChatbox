using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignalRApi.Entities.Room;
using SignalRApi.Entities.User;

namespace SignalRApi.Repositories
{
    public interface IRoomRepository
    {
        public Task<Room> CreateNewRoom(string name, List<string> userIds, CancellationToken cancelationToken = default);
        public Task AddNewRoomToUsers(List<string> userIds, string roomId, CancellationToken cancellationToken = default);
        public Task<List<Room>> GetRoomsForUser(string userId, CancellationToken cancellationToken = default);
        public Task NewMessageForRoom(string roomId, long sentAt, string content, string senderId, CancellationToken cancellationToken = default);
        public Task<Room> GetRoomWithMessages(string roomId, CancellationToken cancellationToken = default);
    }
}

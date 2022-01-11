using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SignalRApi.Entities.Room;
using SignalRApi.Repositories;
using SignalRApi.ViewModels;

namespace SignalRApi.Services
{
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<string> CreateNewRoom(List<string> userIds, string name, CancellationToken cancelationToken = default)
        {
            var newRoom = await _roomRepository.CreateNewRoom(name, userIds, cancelationToken);
            var newRoomId = newRoom.Id;
            await _roomRepository.AddNewRoomToUsers(userIds, newRoomId, cancelationToken);
            return newRoomId;
        }

        public async Task<List<Room>> GetRoomsForUser(string userId, CancellationToken cancellationToken = default)
        {
            return await _roomRepository.GetRoomsForUser(userId, cancellationToken);
        }

        public async Task NewMessageForRoom(string roomId, long sentAt, string content, string senderId, CancellationToken cancellationToken = default)
        {
            await _roomRepository.NewMessageForRoom(roomId, sentAt, content, senderId, cancellationToken);
        }

        public async Task<List<MessageViewModel>> GetMessagesForRoom(string roomId, CancellationToken cancellationToken = default)
        {
            var room = await _roomRepository.GetRoomWithMessages(roomId, cancellationToken);
            return _mapper.Map<List<MessageViewModel>>(room.Messages);
        }

        public async Task SendNotification(string roomId, CancellationToken cancellationToken = default)
        {
            
        }
    }
}

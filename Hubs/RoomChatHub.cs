using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SignalRApi.Services;
using SignalRApi.ViewModels;

namespace SignalRApi.Hubs
{
    [Authorize]
    public class RoomChatHub : Hub
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RoomService _roomService;
        private string _userId => Context?.GetHttpContext().User?.FindFirst("id").Value;

        public RoomChatHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _roomService = _serviceProvider.GetRequiredService<RoomService>();
        }

        /* TODO Making this method become a filter would be better */
        private async Task<bool> CheckCallerBelongsToRoom(string roomId)
        {
            var listRoomsForCurrentUser = await _roomService.GetRoomsForUser(_userId);
            return listRoomsForCurrentUser.Where(room => room.Id == roomId).Any();
        }

        public async Task NotifyJoinRoom(string roomId)
        {
            if (await CheckCallerBelongsToRoom(roomId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            }
            else
            {
                throw new HubException();
            }
        }

        public async Task SendMessage(string roomId, long sentAt, string content)
        {
            if (await CheckCallerBelongsToRoom(roomId))
            {
                await _roomService.NewMessageForRoom(roomId, sentAt, content, _userId);
                await Clients.Group(roomId).SendAsync("onRecievedMessage", new MessageViewModel {
                    Content = content,
                    SentAt = sentAt,
                    SenderId = _userId
                });
                await _roomService.SendNotification(roomId);
            }
            else
            {
                throw new HubException();
            }
        }
    }
}

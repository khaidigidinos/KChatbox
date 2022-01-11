using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SignalRApi.Services;
using SignalRApi.ViewModels;

namespace SignalRApi.Feautures.Room.Queries.GetMessagesForRoomQuery
{
    public class GetMessagesForRoomQuery : IRequest<List<MessageViewModel>>
    {
        public string RoomId { get; set; }
    }

    public class GetMessagesForRoomQueryHandler : IRequestHandler<GetMessagesForRoomQuery, List<MessageViewModel>>
    {
        private readonly RoomService _roomService;

        public GetMessagesForRoomQueryHandler(RoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task<List<MessageViewModel>> Handle(GetMessagesForRoomQuery request, CancellationToken cancellationToken)
        {
            return await _roomService.GetMessagesForRoom(request.RoomId);
        }
    }
}

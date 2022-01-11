using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SignalRApi.DatabaseContexts;
using SignalRApi.Services;

namespace SignalRApi.Feautures.Room.Commands.CreateNewRoomCommand
{
    public class CreateNewRoomCommand : IRequest<string>
    {
        public List<string> ListIds { get; set; }
        public string Name { get; set; }
    }

    public class CreateNewRoomCommandHandler : IRequestHandler<CreateNewRoomCommand, string>
    {
        private readonly RoomService _roomService;

        public CreateNewRoomCommandHandler(RoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task<string> Handle(CreateNewRoomCommand request, CancellationToken cancellationToken)
        {
            var roomId = await _roomService.CreateNewRoom(request.ListIds, request.Name, cancellationToken);
            return roomId;
        }
    }
}

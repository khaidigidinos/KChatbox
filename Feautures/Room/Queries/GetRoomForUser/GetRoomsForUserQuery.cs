using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SignalRApi.DatabaseContexts;
using SignalRApi.Services;
using SignalRApi.ViewModels;

namespace SignalRApi.Feautures.Room.Queries.GetRoomForUser
{
    [Authorize]
    public class GetRoomsForUserQuery : IRequest<List<RoomViewModel>>
    {
    }

    public class GetRoomsForUserQueryHanlder : IRequestHandler<GetRoomsForUserQuery, List<RoomViewModel>>
    {
        private readonly CurrentUserService _currentUserService;
        private readonly RoomService _roomService;
        private readonly IMapper _mapper;

        public GetRoomsForUserQueryHanlder(CurrentUserService currentUserService, RoomService roomService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _roomService = roomService;
            _mapper = mapper;
        }

        public async Task<List<RoomViewModel>> Handle(GetRoomsForUserQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.CurrentUserId();
            var result = await _roomService.GetRoomsForUser(currentUserId);
            return _mapper.Map<List<RoomViewModel>>(result);
        }
    }
}

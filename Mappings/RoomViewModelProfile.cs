using System;
using AutoMapper;
using SignalRApi.Entities.Room;
using SignalRApi.ViewModels;

namespace SignalRApi.Mappings
{
    public class RoomViewModelProfile : Profile
    {
        public RoomViewModelProfile()
        {
            CreateMap<Room, RoomViewModel>();
        }
    }
}

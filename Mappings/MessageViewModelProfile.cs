using System;
using AutoMapper;
using SignalRApi.Entities.Room;
using SignalRApi.ViewModels;

namespace SignalRApi.Mappings
{
    public class MessageViewModelProfile : Profile
    {
        public MessageViewModelProfile()
        {
            CreateMap<Room.Message, MessageViewModel>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SignalRApi.Entities.User;
using SignalRApi.ViewModels;

namespace SignalRApi.Mappings
{
    public class UserViewModelProfile : Profile
    {
        public UserViewModelProfile()
        {
            CreateMap<UserEntity, UserViewModel>()
                .ForMember(vm => vm.Roles, opt => opt.MapFrom(entity => entity.Roles.Select(role => role.Name).ToList()));
        }
    }
}

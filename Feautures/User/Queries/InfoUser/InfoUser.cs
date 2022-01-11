using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SignalRApi.Services;
using SignalRApi.ViewModels;

namespace SignalRApi.Features.User.Queries.InfoUser
{
    [Authorize]
    public class InfoUser : IRequest<UserViewModel>
    {

    }

    public class InfoUserHandler : IRequestHandler<InfoUser, UserViewModel>
    {
        private readonly CurrentUserService _currentUserService;

        public InfoUserHandler(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<UserViewModel> Handle(InfoUser request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_currentUserService.CurrentUserInfo());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SignalRApi.Services;
using SignalRApi.ViewModels;

namespace SignalRApi.Feautures.User.Queries.FindUsersQuery
{
    [Authorize]
    public class FindUsersQuery : IRequest<List<UserViewModel>>
    {
        public string Name { get; set; }
    }

    public class FindUsersQueryHandler : IRequestHandler<FindUsersQuery, List<UserViewModel>>
    {
        private readonly UserService _userService;

        public FindUsersQueryHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<List<UserViewModel>> Handle(FindUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _userService.GetUsersByName(request.Name, cancellationToken);
            return result;
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SignalRApi.Services;

namespace SignalRApi.Features.User.Queries.LoginUser
{
    public class LoginUser : IRequest<string>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
    }

    public class LoginUserHandler : IRequestHandler<LoginUser, string>
    {
        private readonly UserService _userService;

        public LoginUserHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<string> Handle(LoginUser request, CancellationToken cancellationToken)
        {
            return await _userService.Login(request.Name, request.Email, request.Password, request.ClientId, cancellationToken);
        }
    }
}

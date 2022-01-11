using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SignalRApi.Services;

namespace SignalRApi.Feautures.User.Commands.RegisterUser
{
    public class RegisterUser : IRequest<string>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
    }

    public class RegisterUserHanlder : IRequestHandler<RegisterUser, string>
    {
        private readonly UserService _userService;

        public RegisterUserHanlder(UserService userService)
        {
            _userService = userService;
        }

        public async Task<string> Handle(RegisterUser request, CancellationToken cancellationToken)
        {
            return await _userService.Register(request.Name, request.Email, request.Password, request.ClientId, cancellationToken);
        }
    }
}

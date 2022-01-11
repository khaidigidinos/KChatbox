using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SignalRApi.Services;

namespace SignalRApi.Feautures.User.Commands.UpdateFirebaseTokenCommand
{
    public class UpdateFirebaseTokenCommand : IRequest
    {
        public string newToken { get; set; }
        public string oldToken { get; set; }
    }

    public class UpdateFirebaseTokenCommandHanlder : IRequestHandler<UpdateFirebaseTokenCommand>
    {
        private readonly UserService _userService;
        public UpdateFirebaseTokenCommandHanlder(UserService userService)
        {
            _userService = userService;
        }

        public Task<Unit> Handle(UpdateFirebaseTokenCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

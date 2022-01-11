using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using SignalRApi.Services;

namespace SignalRApi.Feautures.User.Commands.RegisterUser
{
    public class RegisterUserValidator : AbstractValidator<RegisterUser>
    {
        private readonly UserService _userService;

        public RegisterUserValidator(UserService userService)
        {
            _userService = userService;

            RuleFor(ins => ins.Name)
                .NotNull().WithMessage("Name cannot be null.")
                .NotEmpty().WithMessage("Name cannot be empty.")
                .MustAsync(BeUniqueName);

            RuleFor(ins => ins.Email)
                .NotNull().WithMessage("Email cannot be null.")
                .NotEmpty().WithMessage("Email cannot be empty.")
                .MustAsync(BeUniqueEmail).WithMessage("Email has been registered.");

            RuleFor(ins => ins.Password)
                .NotNull().WithMessage("Password cannot be null.")
                .NotEmpty().WithMessage("Password cannot be empty.");
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken  cancellationToken = default)
        {
            return (await _userService.GetUserByEmail(email, false, cancellationToken)) is null;
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken = default)
        {
            return (await _userService.GetUsersByName(name, cancellationToken)).Count() <= 0;
        }
    }
}

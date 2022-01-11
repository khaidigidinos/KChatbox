using System;
using FluentValidation;
using System.Linq;
namespace SignalRApi.Feautures.Room.Commands.CreateNewRoomCommand
{
    public class CreateNewRoomCommandValidator : AbstractValidator<CreateNewRoomCommand>
    {
        public CreateNewRoomCommandValidator()
        {
            RuleFor(ins => ins.Name)
                .NotNull().WithMessage("Name cannot be empty")
                .NotEmpty().WithMessage("Name cannot be null");

            RuleFor(ins => ins.ListIds)
                .Must(list => list.Any()).WithMessage("List IDs cannot be null or empty.");
        }
    }
}

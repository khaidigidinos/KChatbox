using System;
using FluentValidation;
namespace SignalRApi.Feautures.Room.Queries.GetMessagesForRoomQuery
{
    public class GetMessagesForRoomQueryValidator : AbstractValidator<GetMessagesForRoomQuery>
    {
        public GetMessagesForRoomQueryValidator()
        {
            RuleFor(ins => ins.RoomId)
                .NotEmpty().WithMessage("RoomId cannot be empty.")
                .NotEmpty().WithMessage("RoomId cannot be null");
        }
    }
}

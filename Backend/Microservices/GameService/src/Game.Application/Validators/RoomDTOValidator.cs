using FluentValidation;
using Game.Application.DTOs;

namespace Game.Application.Validators
{
    public class RoomDTOValidator : AbstractValidator<RoomDTO>
    {
        public RoomDTOValidator()
        {
            RuleFor(r => r.Id)
                .NotEmpty().WithMessage("Id cannot be empty.");

            RuleFor(r => r.RoomType) 
                .IsInEnum().WithMessage("Invalid room type.");

            RuleFor(r => r.RoomStatus) 
                .IsInEnum().WithMessage("Invalid room status.");
        }
    }
}

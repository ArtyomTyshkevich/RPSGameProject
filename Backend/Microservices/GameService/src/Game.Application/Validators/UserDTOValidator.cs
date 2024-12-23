using FluentValidation;
using Game.Application.DTOs;

namespace Game.Application.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(u => u.Id)
                .NotEmpty().WithMessage("Id cannot be empty.");
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(u => u.Status)
                .IsInEnum().WithMessage("Invalid user status.");
            RuleFor(u => u.Rating)
                .GreaterThanOrEqualTo(0).WithMessage("Rating must be a non-negative integer.");
        }
    }
}

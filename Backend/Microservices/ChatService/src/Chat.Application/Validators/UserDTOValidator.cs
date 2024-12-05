using Chat.Application.DTOs;
using FluentValidation;

namespace Chat.Application.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID must not be empty or default GUID.");

            RuleFor(x => x.NickName)
                .NotEmpty()
                .WithMessage("NickName must not be empty.")
                .MaximumLength(15)
                .WithMessage("NickName cannot exceed 15 characters.");
        }
    }
}

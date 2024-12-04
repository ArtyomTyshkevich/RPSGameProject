using FluentValidation;
using Chat.Application.DTOs;

namespace Chat.Application.Validators
{
    public class UserConnectionValidator : AbstractValidator<UserConnection>
    {
        public UserConnectionValidator()
        {
            RuleFor(x => x.UserDTO)
                .NotNull()
                .WithMessage("UserDTO must not be null.")
                .SetValidator(new UserDTOValidator());

            RuleFor(x => x.ChatRoom)
                .NotEmpty()
                .WithMessage("ChatRoom must not be empty.")
                .MaximumLength(20)
                .WithMessage("ChatRoom cannot exceed 20 characters.");
        }
    }
}
using FluentValidation;
using Profile.BLL.DTOs;

namespace Profile.BLL.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(user => user.Id)
                .NotEmpty().WithMessage("User ID cannot be empty.");

            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("Name cannot be empty.")
                .Length(1, 50).WithMessage("Name must be between 1 and 50 characters.");

            RuleFor(user => user.Rating)
                .GreaterThan(0).WithMessage("Rating must be greater than 0.");

            RuleFor(user => user.Mail)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}

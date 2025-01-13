using FluentValidation;
using Profile.BLL.DTOs;

namespace Profile.BLL.Validators
{
    public class RoundDTOValidator : AbstractValidator<RoundDTO>
    {
        public RoundDTOValidator()
        {
            RuleFor(round => round.RoundNumber)
                .GreaterThan(0).WithMessage("Round number must be greater than 0.");

            RuleFor(round => round.FirstPlayerMove)
                .NotEmpty().WithMessage("FirstPlayerMove is required.");

            RuleFor(round => round.SecondPlayerMove)
                .NotEmpty().WithMessage("SecondPlayerMove is required.");

            RuleFor(round => round.RoundResult)
                .NotEmpty().WithMessage("RoundResult is required.");
        }
    }
}

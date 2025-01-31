using FluentValidation;
using Profile.BLL.DTOs;

namespace Profile.BLL.Validators
{
    public class GameDTOValidator : AbstractValidator<GameDTO>
    {
        public GameDTOValidator()
        {
            RuleFor(game => game.Id)
                .NotEmpty().WithMessage("Game ID is required.");

            RuleForEach(game => game.Rounds)
                .SetValidator(new RoundDTOValidator());

            RuleFor(game => game.FirstPlayerId)
                .NotEmpty().WithMessage("FirstPlayerId is required.");

            RuleFor(game => game.SecondPlayerId)
                .NotEmpty().WithMessage("SecondPlayerId is required.");
        }
    }
}

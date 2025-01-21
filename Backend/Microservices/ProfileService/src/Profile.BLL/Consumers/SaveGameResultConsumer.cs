using MassTransit;
using Profile.BLL.Interfaces.Services;
using Profile.DAL.Events;

namespace Profile.BLL.Consumers
{
    public class SaveGameResultConsumer : IConsumer<GameResultsProcessedEvent>
    {
        private readonly IUserService _userService;
        private readonly IGameService _gameService;

        public SaveGameResultConsumer(IUserService userService, IGameService gameService)
        {
            _gameService = gameService;
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<GameResultsProcessedEvent> context)
        {
            var cancellationToken = context.CancellationToken;
            var eventData = context.Message;
            await _userService.UpdateRating(Guid.Parse(eventData.Game.FirstPlayerId!), eventData.FirstPlayerRating, cancellationToken);
            await _userService.UpdateRating(Guid.Parse(eventData.Game.SecondPlayerId!), eventData.SecondPlayerRating, cancellationToken);
            await _gameService.AddGameAsync(eventData.Game, cancellationToken);
        }
    }
}

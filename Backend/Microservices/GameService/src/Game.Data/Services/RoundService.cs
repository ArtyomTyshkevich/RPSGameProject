
using AutoMapper;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Entities;
using Game.Domain.Enums;

namespace Game.Data.Services
{
    public class RoundService : IRoundService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoundService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ProcessRound(Room room, PlayerMoves firstPlayerMove, PlayerMoves secondPlayerMove, CancellationToken cancellationToken)
        {
            var round = room.Rounds[room.RoundNum - 1];
            round.FirstPlayerMove = firstPlayerMove;
            round.SecondPlayerMove = secondPlayerMove;
            round.RoundResult = await _unitOfWork.Rools.GetResultAsync(firstPlayerMove, secondPlayerMove, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

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

        public async Task ProcessRound(Room room, Guid playerId, PlayerMoves move, CancellationToken cancellationToken = default)
        {
            var currentRound = room.Rounds[room.RoundNum];

            await SetPlayerMove(room, currentRound, playerId, move, cancellationToken);

            if (currentRound.FirstPlayerMove.HasValue && currentRound.SecondPlayerMove.HasValue)
            {
                await ProcessRoundResult(room, currentRound, cancellationToken);
            }
        }

        private async Task ProcessRoundResult(Room room, Round currentRound, CancellationToken cancellationToken)
        {
            currentRound.RoundResult = await _unitOfWork.Rools
                .GetResultAsync(
                    currentRound.FirstPlayerMove.GetValueOrDefault(),
                    currentRound.SecondPlayerMove.GetValueOrDefault(),
                    cancellationToken
                );

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (currentRound.RoundResult == GameResults.Draw)
            {
                await ResetRound(currentRound, cancellationToken);
                return;
            }

            var roomWinner = DetermineWinnerRoom(room);
            if (roomWinner != null)
            {
                await ProcessRoomWinner(room, roomWinner.Value, cancellationToken);
            }
            else
            {
                await ProceedToNextRound(room, cancellationToken);
            }
        }

        private async Task ProcessRoomWinner(Room room, GameResults winner, CancellationToken cancellationToken)
        {
            room.GameResult = winner;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await UpdatePlayerRatings(room, cancellationToken);
            await ClearRoom(room, cancellationToken);
        }

        private async Task ProceedToNextRound(Room room, CancellationToken cancellationToken)
        {
            if (room.RoundNum < room.Rounds.Count)
            {
                room.RoundNum++;
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task UpdatePlayerRatings(Room room, CancellationToken cancellationToken)
        {
            if (room.GameResult == GameResults.FirstPlayerWon)
            {
                await _unitOfWork.Users.ChangeReting(room.FirstPlayer!.Id, 25, cancellationToken);
                await _unitOfWork.Users.ChangeReting(room.SecondPlayer!.Id, -25, cancellationToken);
            }
            else if (room.GameResult == GameResults.SecondPlayerWon)
            {
                await _unitOfWork.Users.ChangeReting(room.FirstPlayer!.Id, -25, cancellationToken);
                await _unitOfWork.Users.ChangeReting(room.SecondPlayer!.Id, 25, cancellationToken);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task ClearRoom(Room room, CancellationToken cancellationToken)
        {
            room.FirstPlayer = null;
            room.SecondPlayer = null;
            room.RoundNum = 0;
            room.GameResult = null;
            room.Status = RoomStatuses.InPreparation;
            foreach (var round in room.Rounds)
            {
                round.RoundResult = null;
                round.SecondPlayerMove = null;
                round.FirstPlayerMove = null;
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task SetPlayerMove(Room room, Round currentRound, Guid playerId, PlayerMoves move,CancellationToken cancellationToken)
        {
            if (room.FirstPlayer?.Id == playerId)
            {
                currentRound.FirstPlayerMove = move;
            }
            else if (room.SecondPlayer?.Id == playerId)
            {
                currentRound.SecondPlayerMove = move;
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private GameResults? DetermineWinnerRoom(Room room)
        {
            int totalRounds = room.Rounds.Count;
            int majorityWins = (totalRounds / 2) + 1;

            int firstPlayerWins = room.Rounds.Count(round => round.RoundResult == GameResults.FirstPlayerWon);
            int secondPlayerWins = room.Rounds.Count(round => round.RoundResult == GameResults.SecondPlayerWon);

            if (firstPlayerWins >= majorityWins)
            {
                return GameResults.FirstPlayerWon;
            }
            else if (secondPlayerWins >= majorityWins)
            {
                return GameResults.SecondPlayerWon;
            }
            return null;
        }

        private async Task ResetRound(Round round, CancellationToken cancellationToken)
        {
            round.FirstPlayerMove = null;
            round.SecondPlayerMove = null;
            round.RoundResult = null;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}

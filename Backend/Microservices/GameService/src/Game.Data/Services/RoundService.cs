using AutoMapper;
using Broker.Events;
using Game.Application.DTOs;
using Game.Application.Interfaces.Buses;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Entities;
using Game.Domain.Enums;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;

namespace Game.Data.Services
{
    public class RoundService : IRoundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBrokerBus _bus;
        private readonly IMapper _mapper;
        private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> RoomLocks = new ConcurrentDictionary<Guid, SemaphoreSlim>();

        public RoundService(IUnitOfWork unitOfWork, IBrokerBus bus, IMapper mapper)
        {
            _mapper = mapper;
            _bus = bus;
            _unitOfWork = unitOfWork;
        }

        public async Task<Message?> ProcessRound(Room room, Guid playerId, PlayerMoves move, CancellationToken cancellationToken = default)
        {
            var roundNum = room.RoundNum;
            var currentRound = room.Rounds[roundNum];
            if (playerId == room.FirstPlayer!.Id)
            {
                for (var i = 0; i < 5; i++)
                {
                    room = await _unitOfWork.Rooms.GetByIdAsNoTrakingAsync(room.Id);
                    currentRound = room.Rounds[roundNum];
                    if (currentRound.SecondPlayerMove != null)
                    {
                        break;
                    }
                    await Task.Delay(1000);
                }
            }
            _unitOfWork.Rooms.Attach(room);
            await SetPlayerMove(room, currentRound, playerId, move, cancellationToken);

            if (playerId == room.FirstPlayer!.Id)
            {
                var MovesAreCame = currentRound.FirstPlayerMove!=null && currentRound.SecondPlayerMove!=null;
                if (MovesAreCame)
                {
                    await ProcessRoundResult(room, currentRound, cancellationToken);
                    return new Message
                    {
                        FirstPlayerMoves = currentRound.FirstPlayerMove,
                        SecondPlayerMoves = currentRound.SecondPlayerMove,
                        CurrentRaundNum = roundNum,
                        CurrentRoundWinnerID = GetWinnerId(room.FirstPlayer!.Id, room.SecondPlayer!.Id, currentRound.RoundResult),
                        GameWinnerId = GetWinnerId(room.FirstPlayer!.Id, room.SecondPlayer!.Id, room.GameResult)
                    };
                }
                else return null;
            }
            else return null;

        }
        private Guid? GetWinnerId(Guid firstPlayerId, Guid secondPlayerId, GameResults? gameResults)
        {
            switch (gameResults)
            {
                case GameResults.FirstPlayerWon:
                    return firstPlayerId;

                case GameResults.SecondPlayerWon:
                    return secondPlayerId;

                default:
                    return null;
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
                await GameEndProcess(room, roomWinner.Value, cancellationToken);
            }
            else
            {
                await ProceedToNextRound(room, cancellationToken);
            }
        }

        private async Task GameEndProcess(Room room, GameResults winner, CancellationToken cancellationToken)
        {
            room.GameResult = winner;
            await _unitOfWork.SaveChangesAsync(cancellationToken);;
            await UpdatePlayerRatingsAsync(room, cancellationToken);
            await SaveGameResultAsync(room,cancellationToken);
            await ClearRoomAsync(room, cancellationToken);
        }
        private async Task UpdatePlayerRatingsAsync(Room room, CancellationToken cancellationToken)
        {
            if (room.GameResult == GameResults.FirstPlayerWon)
            {
                await _unitOfWork.Users.ChangeRating(room.FirstPlayer!.Id, 25, cancellationToken);
                await _unitOfWork.Users.ChangeRating(room.SecondPlayer!.Id, -25, cancellationToken);
            }
            else if (room.GameResult == GameResults.SecondPlayerWon)
            {
                await _unitOfWork.Users.ChangeRating(room.FirstPlayer!.Id, -25, cancellationToken);
                await _unitOfWork.Users.ChangeRating(room.SecondPlayer!.Id, 25, cancellationToken);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task SaveGameResultAsync(Room room, CancellationToken cancellationToken)
        {
            await _bus.Publish(new GameResultsProcessedEvent
            {
                FirstPlayerRating = room.FirstPlayer!.Rating,
                SecondPlayerRating = room.SecondPlayer!.Rating,
                Game = _mapper.Map<GameResultDto>(room)

            }, cancellationToken);
        }

        private async Task ClearRoomAsync(Room room, CancellationToken cancellationToken)
        {
            room.Status = RoomStatuses.InPreparation;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task ProceedToNextRound(Room room, CancellationToken cancellationToken)
        {
            if (room.RoundNum < room.Rounds.Count)
            {
                room.RoundNum++;
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
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
        public async Task<Message?> DisconetedAsync(Room room, Guid playerId, CancellationToken cancellationToken = default)
        {
            var roundNum = room.RoundNum;
            var currentRound = room.Rounds[roundNum];
            _unitOfWork.Rooms.Attach(room);
            var IsLeave = room.GameResult == null && room.FirstPlayer != null && room.SecondPlayer != null;
            if (IsLeave)
            {
                var result = GameResults.Draw;
                if (playerId == room.FirstPlayer!.Id)
                {
                    result = GameResults.SecondPlayerWon;
                }
                else if (playerId == room.SecondPlayer!.Id)
                {              
                    result = GameResults.FirstPlayerWon;
                }
                await GameEndProcess(room, result,cancellationToken);

                var gameWiinnerId = GetWinnerId(room.FirstPlayer.Id, room.SecondPlayer.Id, result);

                return new Message
                {
                    FirstPlayerMoves = null,
                    SecondPlayerMoves = null,
                    CurrentRaundNum = roundNum,
                    CurrentRoundWinnerID = gameWiinnerId,
                    GameWinnerId = gameWiinnerId
                };
            }
            else return null;
        }
    }
}

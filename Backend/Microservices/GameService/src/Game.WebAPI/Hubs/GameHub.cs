using Game.Application.Interfaces.Repositories;
using Game.Application.Interfaces.Repositories.UnitOfWork;
using Game.Application.Interfaces.Services;
using Game.Domain.Entities;
using Game.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
public class GameHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoundService _roundService;
    public GameHub(IRoomRepository roomRepository, IUnitOfWork unitOfWork, IRoundService roundService)
    {
        _unitOfWork = unitOfWork;
        _roundService = roundService;
    }

    public async Task SendMove(Guid roomId, Guid playerId, PlayerMoves move, CancellationToken cancellationToken)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);

        if (room == null || (room.FirstPlayer?.Id != playerId && room.SecondPlayer?.Id != playerId))
        {
            throw new Exception("Invalid room or player.");
        }

        var currentRound = room.Rounds[room.RoundNum - 1];

        if (room.FirstPlayer?.Id == playerId)
        {
            currentRound.FirstPlayerMove = move;
        }
        else if (room.SecondPlayer?.Id == playerId)
        {
            currentRound.SecondPlayerMove = move;
        }

        if (currentRound.FirstPlayerMove.HasValue && currentRound.SecondPlayerMove.HasValue)
        {
            currentRound.RoundResult = await _unitOfWork.Rools
                                                    .GetResultAsync
                                                        (
                                                            currentRound.FirstPlayerMove.GetValueOrDefault(),
                                                            currentRound.SecondPlayerMove.GetValueOrDefault(),
                                                            cancellationToken
                                                        );

            await _unitOfWork.SaveChangesAsync();

            if (room.Rounds.Count >= room.RoundNum)
            {
                if (currentRound.RoundResult == GameResults.FirstPlayerWon)
                {
                    await _unitOfWork.Users.ChangeReting(room.FirstPlayer.Id, 25, cancellationToken);
                    await _unitOfWork.Users.ChangeReting(room.SecondPlayer.Id, -25, cancellationToken);
                }
                else if (currentRound.RoundResult == GameResults.SecondPlayerWon)
                {
                    await _unitOfWork.Users.ChangeReting(room.FirstPlayer.Id, -25, cancellationToken);
                    await _unitOfWork.Users.ChangeReting(room.SecondPlayer.Id, 25, cancellationToken);
                }

                room.FirstPlayer = null;
                room.SecondPlayer = null;
                room.Status = RoomStatuses.WaitingPlayers;
                room.Rounds.Clear();
            }

            await _unitOfWork.Rooms.UpdateAsync(room);
            await _unitOfWork.SaveChangesAsync();
        }

        await Clients.Group(roomId.ToString()).SendAsync("ReceiveMove", room);
    }

    public async Task JoinRoom(Guid roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    public async Task LeaveRoom(Guid roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
    }
}
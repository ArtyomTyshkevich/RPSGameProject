using Chat.Data.Context;
using Game.Domain.Entities;
using Game.Domain.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;


namespace Chat.Data.Consumers
{
    public class GameSirchConsumer : IConsumer<User>
    {
        private readonly GameDbContext gameDbContext;

        public GameSirchConsumer(GameDbContext gameDbContext)
        {
            this.gameDbContext = gameDbContext;
        }

        public async Task Consume(ConsumeContext<User> context)
        {
            while (true)
            {
                if (context.Message.Status != UserStatuses.InSearch)
                    break;
                var room = await gameDbContext.Rooms
                .Include(r => r.FirstPlayer)
                    .Include(r => r.SecondPlayer)
                    .FirstOrDefaultAsync(r => r.RoomStatus == RoomStatuses.WaitingPlayers
                                              && (r.FirstPlayer == null || r.SecondPlayer == null));

                if (room != null)
                {
                    if (room.FirstPlayer == null)
                    {
                        room.FirstPlayer = context.Message;
                    }
                    else if (room.SecondPlayer == null)
                    {
                        room.SecondPlayer = context.Message;
                        room.RoomStatus = RoomStatuses.InGame;
                    }

                    await gameDbContext.SaveChangesAsync();
                    break;
                }
                else
                {
                    await Task.Delay(1000);
                }
            }
        }
    }
}
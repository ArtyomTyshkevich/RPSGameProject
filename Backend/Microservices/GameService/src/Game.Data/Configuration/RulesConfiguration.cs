// File: /Configurations/RulesConfiguration.cs
using Game.Domain.Entities;
using Game.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.Data.Configuration
{
    public class RulesConfiguration : IEntityTypeConfiguration<GameRule>
    {
        public void Configure(EntityTypeBuilder<GameRule> builder)
        {
            builder.HasData(
                new GameRule { Id = Guid.NewGuid(), FirstPlayerMove = PlayerMoves.Rock, SecondPlayerMove = PlayerMoves.Scissors, GameResults = GameResults.FirstPlayerWon },
                new GameRule { Id = Guid.NewGuid(), FirstPlayerMove = PlayerMoves.Scissors, SecondPlayerMove = PlayerMoves.Paper, GameResults = GameResults.FirstPlayerWon },
                new GameRule { Id = Guid.NewGuid(), FirstPlayerMove = PlayerMoves.Paper, SecondPlayerMove = PlayerMoves.Rock, GameResults = GameResults.FirstPlayerWon },

                new GameRule { Id = Guid.NewGuid(), FirstPlayerMove = PlayerMoves.Scissors, SecondPlayerMove = PlayerMoves.Rock, GameResults = GameResults.SecondPlayerWon },
                new GameRule { Id = Guid.NewGuid(), FirstPlayerMove = PlayerMoves.Paper, SecondPlayerMove = PlayerMoves.Scissors, GameResults = GameResults.SecondPlayerWon },
                new GameRule { Id = Guid.NewGuid(), FirstPlayerMove = PlayerMoves.Rock, SecondPlayerMove = PlayerMoves.Paper, GameResults = GameResults.SecondPlayerWon },

                new GameRule { Id = Guid.NewGuid(), FirstPlayerMove = PlayerMoves.Rock, SecondPlayerMove = PlayerMoves.Rock, GameResults = GameResults.Draw },
                new GameRule { Id = Guid.NewGuid(), FirstPlayerMove = PlayerMoves.Paper, SecondPlayerMove = PlayerMoves.Paper, GameResults = GameResults.Draw },
                new GameRule { Id = Guid.NewGuid(), FirstPlayerMove = PlayerMoves.Scissors, SecondPlayerMove = PlayerMoves.Scissors, GameResults = GameResults.Draw }
            );
        }
    }
}

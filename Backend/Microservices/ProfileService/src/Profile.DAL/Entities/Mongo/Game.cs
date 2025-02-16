using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Profile.DAL.Entities.Mongo
{
    [BsonIgnoreExtraElements]
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] // Указываем, что идентификатор будет строкой
        public string Id { get; set; }

        [BsonElement("firstPlayer")]
        public string FirstPlayerId { get; set; }

        [BsonElement("secondPlayer")]
        public string SecondPlayerId { get; set; }

        [BsonElement("rounds")]
        public List<Round> Rounds { get; set; }

        [BsonElement("gameResult")]
        public string GameResult { get; set; }

        public Game()
        {
            Id = Guid.NewGuid().ToString();
            FirstPlayerId = string.Empty;
            SecondPlayerId = string.Empty;
            Rounds = new List<Round>();
            GameResult = string.Empty;
        }
    }
}

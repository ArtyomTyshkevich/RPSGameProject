using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Profile.DAL.Entities.Mongo
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("firstPlayer")]
        public string FirstPlayerId { get; set; }

        [BsonElement("secondPlayer")]
        public string SecondPlayerId { get; set; }

        [BsonElement("rounds")]
        public List<Round> Rounds { get; set; }

        [BsonElement("gameResult")]
        public string GameResult { get; set; }

        // Конструктор с инициализацией значений по умолчанию
        public Game()
        {
            Id = ObjectId.GenerateNewId().ToString();  // Генерация ID по умолчанию
            FirstPlayerId = string.Empty;
            SecondPlayerId = string.Empty;
            Rounds = new List<Round>();
            GameResult = string.Empty;
        }
    }
}

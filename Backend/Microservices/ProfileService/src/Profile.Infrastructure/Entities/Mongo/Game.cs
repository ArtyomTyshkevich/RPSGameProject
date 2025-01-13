using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Profile.DAL.Entities.Mongo
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("firstPlayer")]
        public string? FirstPlayerId { get; set; }

        [BsonElement("secondPlayer")]
        public string? SecondPlayerId { get; set; }

        [BsonElement("rounds")]
        public List<Round> Rounds { get; set; } = new List<Round>();

        [BsonElement("gameResult")]
        public string? GameResult { get; set; }
    }
}

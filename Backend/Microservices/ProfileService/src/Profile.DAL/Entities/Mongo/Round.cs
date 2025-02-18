using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Profile.DAL.Entities.Mongo
{
    [BsonIgnoreExtraElements]
    public class Round
    {
        [BsonElement("roundNumber")]
        public int RoundNumber { get; set; }

        [BsonElement("firstPlayerMove")]
        public string FirstPlayerMove { get; set; }

        [BsonElement("secondPlayerMove")]
        public string SecondPlayerMove { get; set; }

        [BsonElement("roundResult")]
        public string RoundResult { get; set; }

        // Конструктор с инициализацией значений по умолчанию
        public Round()
        {
            FirstPlayerMove = string.Empty;
            SecondPlayerMove = string.Empty;
            RoundResult = string.Empty;
        }
    }
}

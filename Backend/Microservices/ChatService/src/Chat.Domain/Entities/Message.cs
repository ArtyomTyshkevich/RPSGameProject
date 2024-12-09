
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();


        [BsonElement("userId")]
        public Guid UserId { get; set; } = Guid.NewGuid();


        [BsonElement("userName")]
        public string UserName { get; set; } = "";


        [BsonElement("content")]
        public string Content { get; set; } = "";

        [BsonElement("SentAt")]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}

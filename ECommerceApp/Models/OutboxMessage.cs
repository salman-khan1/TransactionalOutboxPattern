using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApp.Models
{
    public class OutboxMessage
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string MessageType { get; set; }
        public string MessageBody { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

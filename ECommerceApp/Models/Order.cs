using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ECommerceApp.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

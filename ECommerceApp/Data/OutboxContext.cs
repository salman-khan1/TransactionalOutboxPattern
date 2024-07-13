using ECommerceApp.Models;
using MongoDB.Driver;

namespace ECommerceApp.Data
{
    public class OutboxContext
    {
        private readonly IMongoDatabase _database;

        public OutboxContext(IMongoClient mongoClient, string databaseName)
        {
            _database = mongoClient.GetDatabase(databaseName);
        }

        public IMongoCollection<OutboxMessage> OutboxMessages =>
            _database.GetCollection<OutboxMessage>("OutboxMessages");
    }
}

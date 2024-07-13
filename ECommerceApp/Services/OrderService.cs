using ECommerceApp.Models;
using MassTransit;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace ECommerceApp.Services
{
    public class OrderService
    {
        private readonly OutboxService _outboxService;
        private readonly IBus _bus;
        private readonly IMongoCollection<Order> _orders;

        public OrderService(OutboxService outboxService, IBus bus, IMongoClient mongoClient)
        {
            _outboxService = outboxService;
            _bus = bus;
            var database = mongoClient.GetDatabase("ECommerceDB");
            _orders = database.GetCollection<Order>("Orders");
        }

        public async Task CreateOrderAsync(Order order)
        {
            // Set creation time
            order.CreatedAt = DateTime.UtcNow;

            // Save order to the database
            await _orders.InsertOneAsync(order);

            // Send outbox message
            var endpointUri = new Uri("queue:order-confirmation");
            await _outboxService.SendMessageAsync(order, endpointUri);
        }
    }
}

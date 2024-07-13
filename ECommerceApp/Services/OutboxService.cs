using ECommerceApp.Data;
using ECommerceApp.Models;
using MassTransit;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace ECommerceApp.Services
{
    public class OutboxService
    {
        private readonly OutboxContext _context;
        private readonly IBus _bus;

        public OutboxService(OutboxContext context, IBus bus)
        {
            _context = context;
            _bus = bus;
        }

        public async Task SendMessageAsync<T>(T message, Uri endpointUri) where T : class
        {
            var sendEndpoint = await _bus.GetSendEndpoint(endpointUri);
            var messageBody = JsonConvert.SerializeObject(message);
            var outboxMessage = new OutboxMessage
            {
                MessageType = typeof(T).FullName,
                MessageBody = messageBody,
                CreatedAt = DateTime.UtcNow
            };

            await _context.OutboxMessages.InsertOneAsync(outboxMessage);
            await sendEndpoint.Send(message);
        }
    }
}

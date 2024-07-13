using ECommerceApp.Data;
using ECommerceApp.Models;
using MassTransit;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerceApp.Services
{
    public class OutboxProcessorService : BackgroundService
    {
        private readonly OutboxContext _context;
        private readonly IBus _bus;

        public OutboxProcessorService(OutboxContext context, IBus bus)
        {
            _context = context;
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var unprocessedMessages = await _context.OutboxMessages.Find(m => true).ToListAsync(stoppingToken);
                foreach (var outboxMessage in unprocessedMessages)
                {
                    var endpointUri = new Uri($"queue:{outboxMessage.MessageType}");
                    var sendEndpoint = await _bus.GetSendEndpoint(endpointUri);

                    var messageType = Type.GetType(outboxMessage.MessageType);
                    var message = JsonConvert.DeserializeObject(outboxMessage.MessageBody, messageType);

                    await sendEndpoint.Send(message, stoppingToken);

                    // Optionally, delete or mark the message as processed
                    await _context.OutboxMessages.DeleteOneAsync(m => m.Id == outboxMessage.Id, stoppingToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}

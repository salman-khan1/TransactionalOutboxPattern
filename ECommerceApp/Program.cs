using ECommerceApp.Data;
using ECommerceApp.Services;
using MassTransit;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);


// MongoDB Configuration
var mongoClient = new MongoClient("mongodb://localhost:27017");
builder.Services.AddSingleton<IMongoClient>(mongoClient);

// Registering OutboxContext
builder.Services.AddSingleton(sp => new OutboxContext(sp.GetRequiredService<IMongoClient>(), "ECommerceDB"));

// MassTransit Configuration
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.UseMessageRetry(r => r.Immediate(5));
        cfg.UseInMemoryOutbox();

        cfg.ConfigureEndpoints(context);
    });
});

// Registering Services
builder.Services.AddSingleton<OutboxService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddHostedService<OutboxProcessorService>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using FishingCatalog.msNotification;

var builder = WebApplication.CreateBuilder(args);

// Регистрация RabbitMQService
builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddHostedService<RabbitMQBackgroundService>();

var app = builder.Build();

app.Run();

using FishingCatalog.msNotification.Infrastructure;
using FishingCatalog.msNotification.MessageBroker;

var builder = WebApplication.CreateBuilder(args);

// Регистрация RabbitMQService
builder.Services.AddControllers();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddHostedService<RabbitMQBackgroundService>();

var app = builder.Build();
app.MapControllers();

app.Run();

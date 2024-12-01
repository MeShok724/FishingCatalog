using FishingCatalog.msNotification.MessageBroker;

var builder = WebApplication.CreateBuilder(args);

// ����������� RabbitMQService
builder.Services.AddControllers();
builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddHostedService<RabbitMQBackgroundService>();

var app = builder.Build();
app.MapControllers();

app.Run();

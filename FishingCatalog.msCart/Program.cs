using FishingCatalog.msCart.MessageBroker;
using FishingCatalog.msCart.Repositories;
using FishingCatalog.Postgres;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FishingCatalogDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(FishingCatalogDbContext)));
    });
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddSingleton<RabbitMQService>();
builder.Services.AddHostedService<RabbitMQBackgroundService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    Console.WriteLine("Initalize broker");
    var rabbitMQService = scope.ServiceProvider.GetRequiredService<RabbitMQService>();
    await rabbitMQService.InitializeAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

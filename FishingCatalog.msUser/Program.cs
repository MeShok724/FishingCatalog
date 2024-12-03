using FishingCatalog.msUser.Extensions;
using FishingCatalog.msUser.Infrastructure;
using FishingCatalog.msUser.Repositories;
using FishingCatalog.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

builder.Services.AddAuthentication(
    builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>()
);

//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var rabbitMQService = scope.ServiceProvider.GetRequiredService<RabbitMQService>();
    await rabbitMQService.InitializeAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

using FishingCatalog.msUser.Infrastructure;
using FishingCatalog.msUser.Repositories;
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

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
builder.Services.AddScoped<JwtProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

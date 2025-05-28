using CryptoWallet.Context;
using CryptoWallet.Model;
using CryptoWallet.Repository;
using CryptoWallet.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();

//builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer("Server=(local);Database=CryptoDb_WMW86J;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, BasicUnitOfWork>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddHostedService<PriceUpdaterService>();
builder.Services.AddScoped<ILimitOrderService, LimitOrderService>();


builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

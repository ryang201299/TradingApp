using Microsoft.EntityFrameworkCore;
using TradingApp.Data;
using TradingApp.Helpers.Controllers;
using TradingApp.Helpers.Services;
using TradingApp.Models.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TradingAppContext>(options =>
    options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=TradingApp;Trusted_Connection=True;TrustServerCertificate=True;")
);

// Helpers
builder.Services.AddScoped<IAccountControllerHelper, AccountControllerHelper>();

// Services
builder.Services.AddScoped<IAccountService, AccountService>();

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
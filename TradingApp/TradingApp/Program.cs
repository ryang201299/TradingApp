using TradingApp.Data;
using TradingApp.Models;

using TradingAppContext context = new TradingAppContext();

IQueryable<string> accounts =
    from
        account in context.Accounts

    select
        account.Name;

foreach (string accountName in accounts)
{ 
    Console.WriteLine(accountName);
}


//context.SaveChanges();

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseAuthorization();
//app.MapControllers();

//app.Run();
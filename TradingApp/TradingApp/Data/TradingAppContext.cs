using Microsoft.EntityFrameworkCore;

using TradingApp.Models;

namespace TradingApp.Data;
public class TradingAppContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }

    public DbSet<Security> Securities { get; set; }

    public DbSet<SecurityPrice> SecurityPrices { get; set; }

    public DbSet<TransactionType> TransactionTypes { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=TradingApp;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}
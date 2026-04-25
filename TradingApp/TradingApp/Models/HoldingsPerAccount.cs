using TradingApp.Models.Database;

namespace TradingApp.Models;

public class HoldingsPerAccount
{
    public required Account Account { get; set; }
    public required decimal Holding { get; set; }
}

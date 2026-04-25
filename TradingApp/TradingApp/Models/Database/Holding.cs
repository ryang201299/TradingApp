using System.ComponentModel.DataAnnotations;

namespace TradingApp.Models.Database;
public class Holding
{
    [Key]
    public int HoldingId { get; set; }

    public required Account Account { get; set; }

    public required Security Security { get; set; }

}
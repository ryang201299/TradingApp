using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingApp.Models.Database;

public class Account
{
    [Key]
    public int AccountId { get; set; }

    [MaxLength(50)]
    public required string Name { get; set; }

    public ICollection<Transaction>? Transactions { get; set; }

    [Column(TypeName = "decimal(11,2)")]
    public decimal Cash { get; set; }
}
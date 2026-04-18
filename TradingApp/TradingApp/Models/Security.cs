using System.ComponentModel.DataAnnotations;

namespace TradingApp.Models;
public class Security
{
    [Key]
    public int SecurityId { get; set; }

    [MaxLength(50)]
    public required string SecurityName { get; set; }
}
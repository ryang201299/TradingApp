using System.ComponentModel.DataAnnotations;
using TradingApp.Models.Database;

namespace TradingApp.Models.DTO.Response;

public class TransactionDto
{
    [Required]
    public int TransactionId { get; set; }

    [Required]
    public required AccountDto Account { get; set; }

    [Required]
    public required SecurityDto Security { get; set; }

    [Required]
    public required TransactionTypeDto TransactionType { get; set; }

    [Required]
    public decimal SecurityPrice { get; set; }

    [Required]
    public int Quantity { get; set; }
}
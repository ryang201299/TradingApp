using System.ComponentModel.DataAnnotations;

namespace TradingApp.Models.DTO.Response;
public class TransactionTypeDto
{
    [Required]
    public int TransactionTypeId { get; set; }

    [Required]
    public required string TypeDescription { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace TradingApp.Models.DTO.Response;
public class HoldingsPerAccountDto
{
    [Required]
    public required AccountDto Account { get; set; }

    [Required]
    public decimal Holding { get; set; }
}
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.DTO.Response;
using TradingApp.Models.Interfaces;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Helpers.Controllers;

public class HoldingsControllerHelper : IHoldingsControllerHelper
{
    private readonly ITransactionService _transactionService;
    private readonly ISecurityPriceService _securityPriceService;

    public HoldingsControllerHelper(ITransactionService transactionService, ISecurityPriceService securityPriceService)
    {
        _transactionService = transactionService;
        _securityPriceService = securityPriceService;
    }

    public async Task<List<HoldingsPerAccount>> GetHoldingsAsync()
    {
        // get all transactions, joined with latest prices

        Result<List<Transaction>> transactionsResult = await _transactionService.GetTransactionsAsync();
        List<Transaction> transactions = transactionsResult.Value;

        Result<List<SecurityPrice>> securityPricesResult = await _securityPriceService.GetSecurityPricesAsync();
        List<SecurityPrice> securityPrices = securityPricesResult.Value;

        // group by account and security, and sum of quantity (subtract on sell, add on buy)
        var grouped = transactions
            .GroupBy(x => new
            {
                x.Account,
                x.Security
            })
            .Select(g => new
            {
                Account = g.Key.Account,
                Security = g.Key.Security,
                // Increment buys, decrement sells for total current held quantity
                TotalQuantity = g.Sum(e => e.TransactionType.TransactionTypeId == 1 ? e.Quantity : -e.Quantity )
            }).ToList();

        var joined =
            from
                groupthing in grouped

            join
                securityPrice in securityPrices 
                    on groupthing.Security.SecurityId equals securityPrice.SecurityId

            select new
            {
                Account = groupthing.Account,
                Security = groupthing.Security,
                TotalHeld = groupthing.TotalQuantity * securityPrice.Price
            };

        // Filter out none held securities
        var accountHoldingsNotZero = joined.Where(x => x.TotalHeld != 0).ToList();

        // group by account and sum of holdings

        var accountHoldings = accountHoldingsNotZero.GroupBy(x => x.Account).Select(g => new HoldingsPerAccount()
        { 
            Account = g.Key,
            Holding = g.Sum(e => e.TotalHeld)
        }).ToList();

        return accountHoldings;
    }
}
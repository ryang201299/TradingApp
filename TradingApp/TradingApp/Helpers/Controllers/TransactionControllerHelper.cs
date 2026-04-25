using TradingApp.Helpers.Services;
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.DTO.Request;
using TradingApp.Models.Enums;
using TradingApp.Models.Interfaces;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Helpers.Controllers;

public class TransactionControllerHelper : ITransactionControllerHelper
{
    private readonly ITransactionService _transactionService;
    private readonly IAccountService _accountService;
    private readonly ITransactionTypeService _transactionTypeService;
    private readonly ISecurityService _securityService;
    private readonly ISecurityPriceService _securityPriceService;

    public TransactionControllerHelper(
        ITransactionService transactionService,
        IAccountService accountService,
        ITransactionTypeService transactionTypeService,
        ISecurityService securityService,
        ISecurityPriceService securityPriceService)
    {
        _transactionService = transactionService;
        _accountService = accountService;
        _transactionTypeService = transactionTypeService;
        _securityService = securityService;
        _securityPriceService = securityPriceService;
    }

    public async Task<Result<List<Transaction>>> GetTransactionsAsync()
    {
        return await _transactionService.GetTransactionsAsync();
    }

    public async Task<Result<List<Transaction>>> GetTransactionsAsync(int accountId)
    {
        return await _transactionService.GetTransactionsAsync(accountId);
    }

    public async Task<Result> BuySecurityAsync(TransactionRequestDto request)
    {
        Result<Account?> account = await _accountService.GetAccountAsync(request.AccountId);
        Result<Security?> security = await _securityService.GetSecurityAsync(request.SecurityId);
        Result<TransactionType?> transactionType = await _transactionTypeService.GetTransactionTypeAsync(TransactionTypeEnum.BUY);
        Result<SecurityPrice?> securityPrice = await _securityPriceService.GetSecurityPriceAsync(request.SecurityId);

        if (account.Value is null || security.Value is null || transactionType.Value is null || securityPrice.Value is null)
        {
            return Result.Failure("Failed to retrieve one of the required: account, security, security price, or transaction type.");
        }

        Result result = await _accountService.WidthdrawCashAsync(account.Value, securityPrice.Value.Price * request.Quantity);

        if (!result.IsSuccess)
        {
            return Result.Failure(result.Error);
        }

        Transaction transaction = new()
        {
            Account = account.Value,
            Security = security.Value,
            TransactionType = transactionType.Value,
            SecurityPrice = securityPrice.Value.Price,
            Quantity = request.Quantity
        };

        Result buyResult = await _transactionService.BuySecurityAsync(transaction);

        if (!buyResult.IsSuccess)
        {
            Result refundCashResult = await _accountService.AddCashAsync(account.Value, securityPrice.Value.Price * request.Quantity);

            if (!refundCashResult.IsSuccess) { return Result.Failure("Purchase of shares failed, but cash failed to be refunded to the account."); }
        }

        return Result.Success();
    }

    public async Task<Result> SellSecurityAsync(TransactionRequestDto request)
    {
        Result<Account?> account = await _accountService.GetAccountAsync(request.AccountId);
        Result<Security?> security = await _securityService.GetSecurityAsync(request.SecurityId);
        Result<TransactionType?> transactionType = await _transactionTypeService.GetTransactionTypeAsync(TransactionTypeEnum.SELL);
        Result<SecurityPrice?> securityPrice = await _securityPriceService.GetSecurityPriceAsync(request.SecurityId);

        if (account.Value is null || security.Value is null || transactionType.Value is null || securityPrice.Value is null)
        {
            return Result.Failure("Failed to retrieve one of the required: account, security, security price, or transaction type.");
        }

        // check currently held shares
        Result<List<Transaction>> accountTransactions = await _transactionService.GetTransactionsAsync(request.AccountId);

        if (!accountTransactions.IsSuccess) { return Result.Failure("Failed to check account transactions. Cannot continue with sale."); }
        List<Transaction> relevantTransactions = accountTransactions.Value.Where(x => x.Security.SecurityId == request.SecurityId).ToList();

        // check account holds enough quantity to sell requested amount
        // count of buys
        int countOfBuys = relevantTransactions.Where(x => x.TransactionType.TransactionTypeId == 1).Count();

        // count of sells
        int countOfSells = relevantTransactions.Where(x => x.TransactionType.TransactionTypeId == 2).Count();

        int securitySharesHeld = countOfBuys - countOfSells;

        if (securitySharesHeld < request.Quantity)
        {
            return Result.Failure("Insufficient shares held to complete sale.");
        }

        Transaction transaction = new()
        {
            Account = account.Value,
            Security = security.Value,
            TransactionType = transactionType.Value,
            SecurityPrice = securityPrice.Value.Price,
            Quantity = request.Quantity
        };

        Result sellResult = await _transactionService.SellSecurityAsync(transaction);

        if (!sellResult.IsSuccess) 
        {
            return Result.Failure("Failed to sell shares.");
        }

        // add cash to account
        await _accountService.AddCashAsync(account.Value, request.Quantity * securityPrice.Value.Price);

        return Result.Success();
    }
}

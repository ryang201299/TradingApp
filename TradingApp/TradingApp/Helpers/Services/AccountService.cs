using Microsoft.EntityFrameworkCore;
using TradingApp.Data;
using TradingApp.Models.DTO;
using TradingApp.Models.Interfaces;
using TradingApp.Models;

namespace TradingApp.Helpers.Services;
public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly TradingAppContext _context;

    public AccountService(ILogger<AccountService> logger, TradingAppContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<AccountDto>>> GetAccountsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving accounts...");
            List<AccountDto> accounts = await _context.Accounts
                .Select(x => new AccountDto
                {
                    AccountId = x.AccountId,
                    Name = x.Name
                })
                .ToListAsync();

            _logger.LogInformation("`{CountOfAccounts}` accounts returned.", accounts.Count);

            return Result<List<AccountDto>>.Success(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving accounts.");
            return Result<List<AccountDto>>.Failure(ex.Message);
        }
    }
}
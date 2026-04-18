using TradingApp.Models.DTO;

namespace TradingApp.Models.Interfaces;

/// <summary>
/// Helper class adding additional buisness logic error 
/// handling and logging to the account service class
/// </summary>
public interface IAccountControllerHelper
{
    /// <summary>
    /// Retrieves all accounts
    /// </summary>
    /// <returns>List of accounts and their information</returns>
    Task<Result<List<AccountDto>>> GetAccountsAsync();
}
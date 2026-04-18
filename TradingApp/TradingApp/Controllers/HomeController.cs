using Microsoft.AspNetCore.Mvc;
using TradingApp.Models;
using TradingApp.Models.DTO;
using TradingApp.Models.Interfaces;

namespace TradingApp.Controllers;

/// <summary>
/// Controller housing all endpoints relating to Accounts
/// </summary>
[ApiController]
[Route("accounts")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountControllerHelper _accountHelper;

    public AccountController(ILogger<AccountController> logger, IAccountControllerHelper accountHelper)
    {
        _logger = logger;
        _accountHelper = accountHelper;
    }

    /// <summary>
    /// Retrieves information about accounts
    /// </summary>
    /// <returns>List of accounts</returns>
    [HttpGet]
    public async Task<IActionResult> GetAccounts()
    {
        try
        {
            Result<List<AccountDto>> accountsResult = await _accountHelper.GetAccountsAsync();

            if (accountsResult.IsSuccess)
            {
                return Ok(accountsResult.Value);
            }
            else
            {
                return BadRequest(accountsResult.Error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving accounts.");
            return BadRequest(ex);
        }
    }
}
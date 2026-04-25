namespace TradingApp.Models.Interfaces.ControllerHelpers;
public interface IHoldingsControllerHelper
{
    Task<List<HoldingsPerAccount>> GetHoldingsAsync();
}

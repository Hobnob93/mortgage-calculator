using MortgageCalculator.Core.Enums;

namespace MortgageCalculator.Core.Interfaces;

public interface IWebApiRequest
{
    Task<T?> GetAsync<T>(ApiEndpoint apiEndpoint, params object[] parameters);
}

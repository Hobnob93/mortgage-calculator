using MortgageCalculator.Core.Enums;

namespace MortgageCalculator.Core.Interfaces;

public interface IWebApiRequest
{
    Task<T> GetAsync<T>(ApiEndpoint apiEndpoint, params object[] parameters);
    Task PatchAsync(ApiEndpoint apiEndpoint, object parameter);
    Task DeleteAsync(ApiEndpoint apiEndpoint, object parameter);
}

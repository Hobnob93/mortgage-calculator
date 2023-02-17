using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Core.Interfaces;

public interface IMortgageForecaster
{
    Task<DetailedForecast> GetDetailedForecast();
}

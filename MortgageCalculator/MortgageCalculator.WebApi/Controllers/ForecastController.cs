using Microsoft.AspNetCore.Mvc;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ForecastController : ControllerBase
{
    private readonly IMortgageForecaster _forecaster;

    public ForecastController(IMortgageForecaster forecaster)
    {
        _forecaster = forecaster;
    }

    [HttpGet]
    public async Task<DetailedForecast> Detailed()
    {
        return await _forecaster.GetDetailedForecast();
    }

    [HttpGet]
    public async Task<SimpleForecast> Simple()
    {
        return await _forecaster.GetSimpleForecast(DateOnly.FromDateTime(DateTime.Now));
    }
}

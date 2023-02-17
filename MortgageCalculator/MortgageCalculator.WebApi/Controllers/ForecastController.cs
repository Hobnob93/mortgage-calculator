using Microsoft.AspNetCore.Mvc;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ForecastController : ControllerBase
    {
        private readonly IMortgageForecaster _forecaster;

        public ForecastController(IMortgageForecaster forecaster)
        {
            _forecaster = forecaster;
        }

        [HttpGet]
        public async Task<DetailedForecast> GetDetailedForecast()
        {
            return await _forecaster.GetDetailedForecast();
        }
    }
}

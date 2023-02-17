using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Extensions;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Core.Services;

public class MortgageForecaster : IMortgageForecaster
{
    private readonly IMongoRepository<Mortgage> _mortgagesRepo;
    private readonly IMortgagePaymentsRepository _mortgagePaymentsRepo;

    public MortgageForecaster(IMongoRepository<Mortgage> mortgagesRepo, IMortgagePaymentsRepository mortgagePaymentsRepo)
    {
        _mortgagesRepo = mortgagesRepo;
        _mortgagePaymentsRepo = mortgagePaymentsRepo;
    }

    public async Task<DetailedForecast> GetDetailedForecast()
    {
        // TODO: find mortgage based on forecast start-from date, otherwise use start of FIRST mortgage - need to order by mortgage opened date if multiple mortgages exist
        var mortgage = (await _mortgagesRepo.GetAll()).First();

        var currentDate = mortgage.Opened;
        var amountToPayoff = mortgage.AmountBorrowed;

        var forecast = new DetailedForecast();

        while (amountToPayoff > 0m) 
        {
            var paymentsInMonth = (await _mortgagePaymentsRepo.PaymentsInMonth(currentDate)).ToArray();
            forecast.Months.Add(ForecastNextMonth(ref amountToPayoff, currentDate, mortgage, paymentsInMonth));

            currentDate = currentDate.StartOfNextMonth();
        }

        return forecast;
    }

    private DetailedForecastMonth ForecastNextMonth(ref decimal amountToPayOff, DateOnly date, Mortgage mortgage, MortgagePayment[] payments)
    {

    }
}

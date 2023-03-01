using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Extensions;
using MortgageCalculator.Core.Interfaces;

namespace MortgageCalculator.Core.Services;

public class InterestCalculator : IInterestCalculator
{
    public (decimal ForMonth, decimal ForDay) CalculateInterestForMonth(decimal balance, DateOnly interestStart, Mortgage mortgage, MortgagePayment[] paymentsInMonth)
    {
        var interestPeriod = mortgage.InterestPeriods
            .SingleOrDefault(ip => ip.From <= interestStart && interestStart <= ip.To, mortgage.InterestPeriods.Last());

        var daysInMonth = (interestStart.DaysInMonth() - interestStart.Day) + 1;
        var interestFraction = (interestPeriod.InterestRate / 100m);
        var baseAmount = Math.Round(balance * interestFraction * daysInMonth, 4, MidpointRounding.ToEven);

        var paymentDeductions = 0m;
        foreach (var payment in paymentsInMonth)
        {
            paymentDeductions += Math.Round(payment.Amount * interestFraction * (daysInMonth - payment.PaidOn.Day + 1), 4, MidpointRounding.ToEven);
        }

        var forMonth = Math.Round((baseAmount - paymentDeductions) / (decimal)interestStart.DaysInYear(), 2, MidpointRounding.ToEven);
        var forDay = Math.Round(forMonth / (decimal)daysInMonth, 2, MidpointRounding.ToEven);

        return (forMonth, forDay);
    }
}

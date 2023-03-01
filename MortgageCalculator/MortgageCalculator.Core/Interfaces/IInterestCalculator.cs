using MortgageCalculator.Core.Documents;

namespace MortgageCalculator.Core.Interfaces;

public interface IInterestCalculator
{
    (decimal ForMonth, decimal ForDay) CalculateInterestForMonth(decimal balance, DateOnly interestStart, Mortgage mortgage, MortgagePayment[] paymentsInMonth);
}

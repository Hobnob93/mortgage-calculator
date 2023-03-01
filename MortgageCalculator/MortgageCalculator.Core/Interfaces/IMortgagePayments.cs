using MortgageCalculator.Core.Documents;

namespace MortgageCalculator.Core.Interfaces;

public interface IMortgagePayments
{
    Task<IEnumerable<MortgagePayment>> PaymentsForMonth(Mortgage mortgage, DateOnly date, decimal amountLeftToPay);
}

using MortgageCalculator.Core.Documents;

namespace MortgageCalculator.Core.Interfaces;

public interface IMortgagePaymentsRepository : IMongoRepository<MortgagePayment>
{
    Task<IEnumerable<MortgagePayment>> PaymentsInMonth(DateOnly date);
}

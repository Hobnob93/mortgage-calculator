using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Extensions;
using MortgageCalculator.Core.Interfaces;

namespace MortgageCalculator.Core.Services;

public class MortgagePayments : IMortgagePayments
{
    private readonly IMortgagePaymentsRepository _paymentsRepo;
    private readonly IPublicHolidayChecker _publicHolidays;

    public MortgagePayments(IMortgagePaymentsRepository paymentsRepo, IPublicHolidayChecker publicHolidays)
    {
        _paymentsRepo = paymentsRepo;
        _publicHolidays = publicHolidays;
    }

    public async Task<IEnumerable<MortgagePayment>> PaymentsForMonth(Mortgage mortgage, DateOnly date, decimal amountLeftToPay)
    {
        var payments = (await _paymentsRepo.PaymentsInMonth(date)).ToList();

        var paymentDate = DeterminePaymentDate(date);
        if (paymentDate == mortgage.FirstPaymentDate)
        {
            payments.Add(new MortgagePayment { Amount = mortgage.FirstPaymentAmount, PaidOn = paymentDate });
        }
        else if (date > mortgage.FirstPaymentDate)
        {
            var interestPeriod = mortgage.InterestPeriods
                .SingleOrDefault(ip => ip.From <= date && date <= ip.To, mortgage.InterestPeriods.Last());

            var paymentAmount = amountLeftToPay > interestPeriod.MonthlyPayment
                ? interestPeriod.MonthlyPayment
                : amountLeftToPay;

            payments.Add(new MortgagePayment { Amount = paymentAmount, PaidOn = paymentDate });
        }

        return payments;
    }

    private DateOnly DeterminePaymentDate(DateOnly date)
    {
        date = new DateOnly(date.Year, date.Month, 1);

        while (date.IsWeekend() || _publicHolidays.IsBankHoliday(date))
            date = date.AddDays(1);

        return date;
    }
}

namespace MortgageCalculator.Core.Interfaces;

public interface IPublicHolidayChecker
{
    bool IsBankHoliday(DateOnly date);
    DateOnly EasterMonday(int year);
}

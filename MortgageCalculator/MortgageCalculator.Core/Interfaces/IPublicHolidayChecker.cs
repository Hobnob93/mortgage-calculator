namespace MortgageCalculator.Core.Interfaces;

public interface IPublicHolidayChecker
{
    bool IsBankHoliday(DateOnly date);
    DateOnly NewYearsDay(int year);
    DateOnly GoodFriday(int year);
    DateOnly EasterMonday(int year);
    DateOnly MayDay(int year);
    DateOnly SpringDay(int year);
    DateOnly SummerDay(int year);
    DateOnly BoxingDay(int year);
}

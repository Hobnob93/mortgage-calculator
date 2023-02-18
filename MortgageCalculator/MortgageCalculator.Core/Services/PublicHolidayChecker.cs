using MortgageCalculator.Core.Extensions;
using MortgageCalculator.Core.Interfaces;

namespace MortgageCalculator.Core.Services;

public class PublicHolidayChecker : IPublicHolidayChecker
{
    public bool IsBankHoliday(DateOnly date)
    {
        return false;
    }

    public DateOnly NewYearsDay(int year)
    {
        return new DateOnly(year, 1, 1).AdjustedForWeekend();
    }

    public DateOnly GoodFriday(int year)
    {
        return EasterSunday(year).AddDays(-2);
    }

    public DateOnly EasterMonday(int year)
    {
        return EasterSunday(year).AddDays(1);
    }

    public DateOnly MayDay(int year)
    {
        var dayInMay = new DateOnly(year, 5, 1);

        while (dayInMay.DayOfWeek != DayOfWeek.Monday)
            dayInMay = dayInMay.AddDays(1);

        return dayInMay;
    }

    public DateOnly SpringDay(int year)
    {
        var firstMonday = MayDay(year);

        var fourWeeksLater = firstMonday.AddDays(28);
        if (fourWeeksLater.Month != firstMonday.Month)
            return firstMonday.AddDays(21);

        return fourWeeksLater;
    }

    public DateOnly SummerDay(int year)
    {
        var dayInAugust = new DateOnly(year, 8, 31);

        while (dayInAugust.DayOfWeek != DayOfWeek.Monday)
            dayInAugust = dayInAugust.AddDays(-1);

        return dayInAugust;
    }

    public DateOnly ChristmasDay(int year)
    {
        var boxingDay = new DateOnly(year, 12, 26);
        if (boxingDay.DayOfWeek == DayOfWeek.Monday)
            return boxingDay.AddDays(1);

        return new DateOnly(year, 12, 25).AdjustedForWeekend();
    }

    public DateOnly BoxingDay(int year)
    {
        var expectedDay = new DateOnly(year, 12, 26);
        if (expectedDay.DayOfWeek == DayOfWeek.Monday)
            return expectedDay;

        var christmasDay = new DateOnly(year, 12, 25).AdjustedForWeekend();
        if (christmasDay.DayOfWeek == DayOfWeek.Monday && expectedDay.DayOfWeek == DayOfWeek.Sunday)
            return christmasDay.AddDays(1);

        return expectedDay.AdjustedForWeekend();
    }

    private DateOnly EasterSunday(int year)
    {
        // Gauss's Easter Algorithm
        var a = year % 19;
        var b = year % 4;
        var c = year % 7;
        var k = year / 100;
        var p = (13 + (8 * k)) / 25;
        var q = k / 4;
        var M = (15 - p + k - q) % 30;
        var N = (4 + k - q) % 7;
        var d = (19 * a + M) % 30;
        var e = ((2 * b) + (4 * c) + (6 * d) + N) % 7;

        var marchDay = 22 + d + e;
        if (marchDay <= 31)
            return new DateOnly(year, 3, marchDay);

        var aprilDay = d + e - 9;
        if (aprilDay == 25 && d == 28 && e == 6 && ((11 * M + 11) % 30) < 19)
            aprilDay = 18;

        if (aprilDay == 26 && d == 29 && e == 6)
            aprilDay = 19;

        return new DateOnly(year, 4, aprilDay);
    }
}

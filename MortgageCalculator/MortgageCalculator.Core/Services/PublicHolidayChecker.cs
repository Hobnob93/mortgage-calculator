using MortgageCalculator.Core.Interfaces;

namespace MortgageCalculator.Core.Services;

public class PublicHolidayChecker : IPublicHolidayChecker
{
    public bool IsBankHoliday(DateOnly date)
    {
        return false;
    }

    public DateOnly EasterMonday(int year)
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

        var marchDay = 22 + d + e;       // Sunday
        if (marchDay <= 31)
            return new DateOnly(year, 3, marchDay + 1);

        var aprilDay = d + e - 9;        // Sunday
        if (aprilDay == 25 && d == 28 && e == 6 && ((11 * M + 11) % 30) < 19)
            aprilDay = 18;

        if (aprilDay == 26 && d == 29 && e == 6)
            aprilDay = 19;

        return new DateOnly(year, 4, aprilDay + 1);
    }
}

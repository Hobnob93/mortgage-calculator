using Microsoft.VisualBasic;
using MongoDB.Bson.Serialization.Conventions;
using System.Runtime.CompilerServices;

namespace MortgageCalculator.Core.Extensions;

public static class DateOnlyExtensions
{
    public static int DaysInMonth(this DateOnly date)
    {
        return DateTime.DaysInMonth(date.Year, date.Month);
    }

    public static (DateOnly, DateOnly) FirstAndLastDaysOfMonth(this DateOnly date)
    {
        var daysInMonth = date.DaysInMonth();
        var firstOfMonth = new DateOnly(date.Year, date.Month, 1);
        var lastOfMonth = new DateOnly(date.Year, date.Month, daysInMonth);

        return (firstOfMonth, lastOfMonth);
    }

    public static DateOnly StartOfNextMonth(this DateOnly thisMonth)
    {
        return thisMonth
            .AddDays(-(thisMonth.Day - 1))
            .AddMonths(1);
    }

    public static DateOnly AdjustedForWeekend(this DateOnly date)
    {
        if (date.DayOfWeek == DayOfWeek.Sunday)
            return date.AddDays(1);

        if (date.DayOfWeek == DayOfWeek.Saturday)
            return date.AddDays(2);

        return date;
    }

    public static bool IsWeekend(this DateOnly date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    public static int DaysInYear(this DateOnly date)
    {
        var daysInFeb = new DateOnly(date.Year, 2, 1).DaysInMonth();
        if (daysInFeb == 28)
            return 365;

        return 366;
    }
}

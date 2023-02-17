﻿using Microsoft.VisualBasic;
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
}

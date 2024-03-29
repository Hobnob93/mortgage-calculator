﻿namespace MortgageCalculator.Core.Documents;

public record MortgagePayment : DocumentBase
{
    public decimal Amount { get; set; }
    public DateOnly PaidOn { get; set; }
    public MortgagePaymentTo? PaidTo { get; init; }
    public bool IsOverPayment { get; set; }
}

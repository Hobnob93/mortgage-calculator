﻿namespace MortgageCalculator.Core.Models;

public record House : DocumentBase
{
    public required string Address1 { get; init; }
    public required string Address2 { get; init; }
    public required string City { get; init; }
    public required string Postcode { get; init; }
    public decimal PurchasedValue { get; set; }
    public decimal EstimatedValue { get; set; }
}

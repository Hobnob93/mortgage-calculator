﻿using Microsoft.AspNetCore.Mvc;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MortgageController : ControllerBase
{
    private readonly IMortgageForecaster _mortgageForecaster;
    private readonly IMortgageRepository _mortgageRepo;

    public MortgageController(IMortgageRepository mortgageRepo, IMortgageForecaster mortgageForecaster)
    {
        _mortgageRepo = mortgageRepo;
        _mortgageForecaster = mortgageForecaster;
    }

    [HttpGet]
    public async Task<EstimatedHouseValue> HouseValue()
    {
        var mortgage = await _mortgageRepo.GetMostRecentMortgage();
        var house = mortgage.House ?? throw new InvalidOperationException("Most recent mortgage has no associated house!");
        var simpleForecast = await _mortgageForecaster.GetSimpleForecast(DateOnly.FromDateTime(DateTime.Now));

        return new EstimatedHouseValue
        {
            EstimatedValue = house.EstimatedValue,
            PurchaseValue = house.PurchasedValue,
            Equity = house.EstimatedValue - simpleForecast.Balance,
            PercentageDifference = (double)(((house.EstimatedValue - house.PurchasedValue) / house.PurchasedValue) * 100m)
        };
    }
}

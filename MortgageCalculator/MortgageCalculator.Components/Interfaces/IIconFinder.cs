using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Components.Interfaces;

public interface IIconFinder
{
    Task<IconData[]> GetSelectableIcons();
}

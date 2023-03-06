using MortgageCalculator.Components.BaseComponents;
using MortgageCalculator.Core.Enums;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Components.Layout;

public partial class LandingDashboard : ApiComponentBase
{
    private EstimatedHouseValue? houseValue;

    protected override async Task OnInitializedAsync()
    {
        houseValue = await ApiGet<EstimatedHouseValue>(ApiEndpoint.HouseValue);
    }
}

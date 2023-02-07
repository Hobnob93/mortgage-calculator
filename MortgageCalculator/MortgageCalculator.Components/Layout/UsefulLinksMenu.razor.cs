using MortgageCalculator.Components.BaseComponents;
using MortgageCalculator.Core.Enums;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Components.Layout;

public partial class UsefulLinksMenu : ApiComponentBase
{
    private UsefulLink[] UsefulLinks { get; set; } = Array.Empty<UsefulLink>();

    protected override async Task OnInitializedAsync()
    {
        UsefulLinks = await ApiGet<UsefulLink[]>(ApiEndpoint.UsefulLinks)
            ?? Array.Empty<UsefulLink>();
    }
}

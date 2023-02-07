using Microsoft.AspNetCore.Components;
using MortgageCalculator.Components.BaseComponents;
using MortgageCalculator.Components.Dialogs;
using MortgageCalculator.Core.Enums;
using MortgageCalculator.Core.Models;
using MudBlazor;

namespace MortgageCalculator.Components.Layout;

public partial class UsefulLinksMenu : ApiComponentBase
{
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    private UsefulLink[] UsefulLinks { get; set; } = Array.Empty<UsefulLink>();

    protected override async Task OnInitializedAsync()
    {
        await LoadUsefulLinks();
    }

    private async Task LoadUsefulLinks()
    {
        UsefulLinks = (await ApiGet<UsefulLink[]>(ApiEndpoint.UsefulLinks)
            ?? Enumerable.Empty<UsefulLink>())
            .OrderBy(l => l.Name)
            .ToArray();
    }

    private void ShowDialog()
    {
        var parameters = new DialogParameters 
        {
            [nameof(UsefulLinkEntries.UsefulLinks)] = UsefulLinks
        };

        DialogService.Show<UsefulLinkEntries>("Manage & Maintain Useful Links", parameters);
    }
}

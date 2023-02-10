using Microsoft.AspNetCore.Components;
using MortgageCalculator.Components.BaseComponents;
using MortgageCalculator.Components.Dialogs;
using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Enums;
using MudBlazor;

namespace MortgageCalculator.Components.Layout;

public partial class UsefulLinksMenu : ApiComponentBase
{
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    private List<UsefulLink> UsefulLinks { get; set; } = Enumerable.Empty<UsefulLink>().ToList();

    protected override async Task OnInitializedAsync()
    {
        await LoadUsefulLinks();
    }

    private async Task LoadUsefulLinks()
    {
        UsefulLinks = (await ApiGet<List<UsefulLink>>(ApiEndpoint.UsefulLinks)
            ?? Enumerable.Empty<UsefulLink>())
            .OrderBy(l => l.Name)
            .ToList();
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

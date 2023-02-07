using Microsoft.AspNetCore.Components;
using MortgageCalculator.Core.Models;
using MudBlazor;

namespace MortgageCalculator.Components.Dialogs;

public partial class UsefulLinkEntries : ComponentBase
{
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [CascadingParameter]
    private MudDialogInstance Dialog { get; set; } = default!;

    [Parameter, EditorRequired]
    public UsefulLink[] UsefulLinks { get; set; } = default!;

    private async Task RowClickEvent(TableRowClickEventArgs<UsefulLink> tableRowClickEventArgs)
    {
        var parameters = new DialogParameters
        {
            [nameof(UsefulLinkEntry.UsefulLink)] = tableRowClickEventArgs.Item
        };

        var entryDialog = await DialogService.ShowAsync<UsefulLinkEntry>($"Manage {tableRowClickEventArgs.Item.Name}", parameters);
        var result = await entryDialog.Result;

        if (!result.Canceled)
        {
            Snackbar.Add("Link Saved!", Severity.Success);
        }
    }
}

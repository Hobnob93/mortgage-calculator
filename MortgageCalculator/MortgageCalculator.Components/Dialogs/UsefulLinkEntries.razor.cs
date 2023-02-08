using Microsoft.AspNetCore.Components;
using MortgageCalculator.Core.Enums;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Models;
using MudBlazor;

namespace MortgageCalculator.Components.Dialogs;

public partial class UsefulLinkEntries : ComponentBase
{
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [Inject]
    private IWebApiRequest WebApiRequest { get; set; } = default!;

    [CascadingParameter]
    private MudDialogInstance Dialog { get; set; } = default!;

    [Parameter, EditorRequired]
    public UsefulLink[] UsefulLinks { get; set; } = default!;

    private async Task RowClickEvent(TableRowClickEventArgs<UsefulLink> tableRowClickEventArgs)
    {
        var parameters = new DialogParameters
        {
            [nameof(UsefulLinkEntry.UsefulLink)] = tableRowClickEventArgs.Item with { Name = "Change Me" }     // Shallow copy
        };

        var entryDialog = await DialogService.ShowAsync<UsefulLinkEntry>($"Manage {tableRowClickEventArgs.Item.Name}", parameters);
        var result = await entryDialog.Result;

        if (!result.Canceled)
        {
            try
            {
                var resultLink = (UsefulLink)result.Data
                ?? throw new InvalidCastException("Could not cast data result into useful link");

                await WebApiRequest.PatchAsync(ApiEndpoint.UsefulLinks, resultLink);

                var linkIndex = Array.FindIndex(UsefulLinks, l => l.Id == resultLink.Id);
                UsefulLinks[linkIndex] = resultLink;

                Snackbar.Add("Link Saved!", Severity.Success);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error trying to save link: {ex.Message}");
            }
        }
    }
}

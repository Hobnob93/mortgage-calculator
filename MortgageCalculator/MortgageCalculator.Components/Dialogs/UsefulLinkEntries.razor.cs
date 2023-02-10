using Microsoft.AspNetCore.Components;
using MortgageCalculator.Core.Enums;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Documents;
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
    public List<UsefulLink> UsefulLinks { get; set; } = default!;

    private async Task RowClickEvent(TableRowClickEventArgs<UsefulLink> tableRowClickEventArgs)
    {
        await ShowManageLinkDialog(tableRowClickEventArgs.Item);
    }

    private async Task CreateNewLink()
    {
        await ShowManageLinkDialog(new UsefulLink
        { 
            Href = string.Empty,
            Name = string.Empty,
            IconName = "Link",
            Icon = Icons.Material.Filled.Link
        });
    }

    private async Task ShowManageLinkDialog(UsefulLink link)
    {
        var parameters = new DialogParameters
        {
            [nameof(UsefulLinkEntry.UsefulLink)] = link with { }     // Shallow copy
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium
        };

        var entryDialog = await DialogService.ShowAsync<UsefulLinkEntry>($"Manage {link.Name}", parameters, options);
        var result = await entryDialog.Result;

        if (!result.Canceled)
        {
            try
            {
                var resultLink = (UsefulLink)result.Data
                    ?? throw new InvalidCastException("Could not cast data result into useful link");

                await WebApiRequest.PatchAsync(ApiEndpoint.UsefulLinks, resultLink);

                var linkIndex = UsefulLinks.FindIndex(l => l.Id == resultLink.Id);
                if (linkIndex != -1)
                    UsefulLinks[linkIndex] = resultLink;
                else
                    UsefulLinks.Add(resultLink);

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

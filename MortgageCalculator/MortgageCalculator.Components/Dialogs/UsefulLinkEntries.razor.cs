using Microsoft.AspNetCore.Components;
using MortgageCalculator.Core.Enums;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Documents;
using MudBlazor;
using MortgageCalculator.Core.Models;

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
                if (result.Data is EditUsefulLinkResult editResult)
                    await HandleEditResult(editResult);
                else
                    throw new InvalidCastException("Could not cast data result into useful link");

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error trying to save link: {ex.Message}");
            }
        }
    }

    private async Task HandleEditResult(EditUsefulLinkResult result)
    {
        if (result.EditResult == EditResult.New || result.EditResult == EditResult.Changed)
            await HandleSavedLink(result.UsefulLink);
        else
            await HandleDeletion(result.UsefulLink);
    }

    private async Task HandleDeletion(UsefulLink link)
    {
        await WebApiRequest.DeleteAsync(ApiEndpoint.UsefulLinks, link);

        UsefulLinks.RemoveAll(l => l.Id == link.Id);
        Snackbar.Add("Link Deleted!", Severity.Info);
    }

    private async Task HandleSavedLink(UsefulLink link)
    {
        await WebApiRequest.PatchAsync(ApiEndpoint.UsefulLinks, link);

        var linkIndex = UsefulLinks.FindIndex(l => l.Id == link.Id);
        if (linkIndex != -1)
            UsefulLinks[linkIndex] = link;
        else
            UsefulLinks.Add(link);

        Snackbar.Add("Link Saved!", Severity.Success);
    }
}

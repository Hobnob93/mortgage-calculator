using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Models;
using MudBlazor;

namespace MortgageCalculator.Components.Dialogs;

public partial class UsefulLinkEntry : ComponentBase
{
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    [CascadingParameter]
    private MudDialogInstance Dialog { get; set; } = default!;

    [Parameter, EditorRequired]
    public UsefulLink UsefulLink { get; set; } = default!;

    private UsefulLinkFormModel _model = new();

    protected override void OnParametersSet()
    {
        _model = new UsefulLinkFormModel
        {
            Id = UsefulLink.Id,
            Name = UsefulLink.Name,
            Href = UsefulLink.Href,
            Icon = UsefulLink.Icon,
            IconName = UsefulLink.IconName
        };
    }

    private void OnValidSubmit(EditContext context)
    {
        Dialog.Close(DialogResult.Ok(new UsefulLink
        { 
            Id = _model.Id,
            Name = _model.Name,
            Href = _model.Href,
            Icon = _model.Icon,
            IconName = _model.IconName
        }));
    }

    private async Task OnChangeIconClicked()
    {
        var parameters = new DialogParameters
        {
            [nameof(IconSelection.Model)] = new IconData
            {
                Icon = _model.Icon,
                IconName = _model.IconName
            }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            NoHeader = true
        };

        var iconDialog = await DialogService.ShowAsync<IconSelection>($"Select an Icon", parameters, options);
        var result = await iconDialog.Result;

        if (!result.Canceled)
        {
            try
            {
                var resultLink = (IconData)result.Data
                    ?? throw new InvalidCastException("Could not cast data result into icon data");

                _model.Icon = resultLink.Icon;
                _model.IconName = resultLink.IconName;

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error trying to change icon: {ex.Message}");
            }
        }
    }
}

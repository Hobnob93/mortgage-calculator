using Microsoft.AspNetCore.Components;
using MortgageCalculator.Components.Interfaces;
using MortgageCalculator.Core.Models;
using MudBlazor;

namespace MortgageCalculator.Components.Dialogs;

public partial class IconSelection : ComponentBase
{
    [Inject]
    private IIconFinder IconFinder { get; set; } = default!;

    [CascadingParameter]
    private MudDialogInstance Dialog { get; set; } = default!;

    [Parameter, EditorRequired]
    public IconData Model { get; set; } = default!;

    private IEnumerable<IconData> _selectableIcons = Enumerable.Empty<IconData>();

    private string _searchString = string.Empty;
    private Func<IconData, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (x.IconName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    protected override async Task OnParametersSetAsync()
    {
        _selectableIcons = await IconFinder.GetSelectableIcons();
    }

    private void RowClicked(TableRowClickEventArgs<IconData> args)
    {
        Dialog.Close(DialogResult.Ok(args.Item));
    }
}

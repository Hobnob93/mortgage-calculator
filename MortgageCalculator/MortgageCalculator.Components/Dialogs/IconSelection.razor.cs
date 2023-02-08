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

    private IconData[] _selectableIcons = Array.Empty<IconData>();

    protected override async Task OnParametersSetAsync()
    {
        _selectableIcons = await IconFinder.GetSelectableIcons();
        Console.WriteLine($"Size: {_selectableIcons.Length}");
    }
}

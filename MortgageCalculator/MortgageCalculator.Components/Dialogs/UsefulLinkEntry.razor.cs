using Microsoft.AspNetCore.Components;
using MortgageCalculator.Core.Models;
using MudBlazor;

namespace MortgageCalculator.Components.Dialogs;

public partial class UsefulLinkEntry : ComponentBase
{
    [CascadingParameter]
    private MudDialogInstance Dialog { get; set; } = default!;

    [Parameter, EditorRequired]
    public UsefulLink UsefulLink { get; set; } = default!;
}

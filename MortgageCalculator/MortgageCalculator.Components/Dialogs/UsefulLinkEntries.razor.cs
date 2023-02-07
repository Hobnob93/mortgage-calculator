using Microsoft.AspNetCore.Components;
using MortgageCalculator.Core.Models;
using MudBlazor;

namespace MortgageCalculator.Components.Dialogs;

public partial class UsefulLinkEntries : ComponentBase
{
    [CascadingParameter]
    private MudDialogInstance Dialog { get; set; } = default!;

    [Parameter, EditorRequired]
    public UsefulLink[] UsefulLinks { get; set; } = default!;

    private void RowClickEvent(TableRowClickEventArgs<UsefulLink> tableRowClickEventArgs)
    {
        
    }
}

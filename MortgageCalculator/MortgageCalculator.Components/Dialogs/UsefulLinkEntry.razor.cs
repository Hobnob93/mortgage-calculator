using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Models;
using MudBlazor;

namespace MortgageCalculator.Components.Dialogs;

public partial class UsefulLinkEntry : ComponentBase
{
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
}

using Microsoft.AspNetCore.Components;
using MortgageCalculator.Components.Enums;
using MudBlazor;

namespace MortgageCalculator.Components.Container;

public partial class DashboardNumber
{
    [Parameter, EditorRequired]
    public int ItemSize { get; set; }

    [Parameter]
    public ColorCode ColorCode { get; set; } = ColorCode.Default;

    [Parameter, EditorRequired]
    public string Caption { get; set; } = default!;

    [Parameter, EditorRequired]
    public string ValueAsString { get; set; } = default!;

    [Parameter]
    public string? LeadingText { get; set; }

    [Parameter]
    public string? TrailingText { get; set; }

    private Color Color => ColorCode switch
    {
        ColorCode.Neutral => Color.Info,
        ColorCode.Good => Color.Success,
        ColorCode.Bad => Color.Error,
        ColorCode.Caution => Color.Warning,
        _ => Color.Default
    };
}

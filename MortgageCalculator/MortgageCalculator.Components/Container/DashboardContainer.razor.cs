using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using MortgageCalculator.Components.Enums;

namespace MortgageCalculator.Components.Container;

public partial class DashboardContainer : ComponentBase
{
    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public VerticalHeight VerticalHeight { get; set; } = VerticalHeight.Auto;

    private string CardStyling => new StyleBuilder()
        .AddStyle("height", $"{(int)VerticalHeight}vh", when: VerticalHeight != VerticalHeight.Auto)
        .Build();

    private string CardClasses => new CssBuilder()
        .AddClass("d-flex")
        .AddClass("flex-column")
        .Build();

    private string CardContentClasses => new CssBuilder()
        .AddClass("d-flex")
        .AddClass("align-center")
        .AddClass("justify-center")
        .AddClass("flex-1")
        .Build();
}

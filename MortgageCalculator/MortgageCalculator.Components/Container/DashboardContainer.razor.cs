using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using MortgageCalculator.Components.Enums;

namespace MortgageCalculator.Components.Container;

public partial class DashboardContainer : ComponentBase
{
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public VerticalHeight VerticalHeight { get; set; } = VerticalHeight.Auto;

    private string Styling => new StyleBuilder()
        .AddStyle("height", $"{(int)VerticalHeight}vh", when: VerticalHeight != VerticalHeight.Auto)
        .Build();
}

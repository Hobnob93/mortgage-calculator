using Microsoft.AspNetCore.Components;

namespace MortgageCalculator.Components.Container;

public partial class LandingContainer : ComponentBase
{
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = default!;
}

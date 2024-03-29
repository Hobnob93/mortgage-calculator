﻿using Microsoft.AspNetCore.Components;
using MortgageCalculator.Components.Enums;
using MortgageCalculator.Components.Interfaces;

namespace MortgageCalculator.Components.Container;

public partial class DashboardContainer : ComponentBase
{
    [Inject]
    protected IHtmlBuilderFactory HtmlBuilderFactory { get; private set; } = default!;

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool IsLoading { get; set; }

    [Parameter]
    public VerticalHeight VerticalHeight { get; set; } = VerticalHeight.Auto;

    private string CardStyling => HtmlBuilderFactory.NewCssBuilder()
        .Add("height", $"{(int)VerticalHeight}vh", condition: VerticalHeight != VerticalHeight.Auto)
        .Build();

    private string CardClasses => HtmlBuilderFactory.NewClassBuilder()
        .Add("d-flex")
        .Add("flex-column")
        .Build();

    private string CardContentClasses => HtmlBuilderFactory.NewClassBuilder()
        .Add("d-flex")
        .Add("align-center")
        .Add("justify-center")
        .Add("flex-1")
        .Add("pt-2")
        .Build();

    private string CardHeaderClasses => HtmlBuilderFactory.NewClassBuilder()
        .Add("pb-2")
        .Build();
}

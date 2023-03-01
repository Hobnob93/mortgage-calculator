using MortgageCalculator.Components.Builders;

namespace MortgageCalculator.Components.Interfaces;

public interface IHtmlBuilderFactory
{
    CssBuilder NewCssBuilder();
    CssBuilder NewCssBuilder(string css);
    CssBuilder NewCssBuilder(CssBuilder builder);

    HtmlClassBuilder NewClassBuilder();
    HtmlClassBuilder NewClassBuilder(string classes);
    HtmlClassBuilder NewClassBuilder(HtmlClassBuilder builder);
}

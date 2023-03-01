using MortgageCalculator.Components.Builders;
using MortgageCalculator.Components.Interfaces;

namespace MortgageCalculator.Components.Services;

public class HtmlBuilderFactory : IHtmlBuilderFactory
{
    public CssBuilder NewCssBuilder()
    {
        return new CssBuilder();
    }

    public CssBuilder NewCssBuilder(string css)
    {
        return new CssBuilder(css);
    }

    public CssBuilder NewCssBuilder(CssBuilder builder)
    {
        return new CssBuilder(builder);
    }

    public HtmlClassBuilder NewClassBuilder()
    {
        return new HtmlClassBuilder();
    }

    public HtmlClassBuilder NewClassBuilder(string classes)
    {
        return new HtmlClassBuilder(classes);
    }

    public HtmlClassBuilder NewClassBuilder(HtmlClassBuilder builder)
    {
        return new HtmlClassBuilder(builder);
    }
}

using MortgageCalculator.Components.Interfaces;
using System.Text;

namespace MortgageCalculator.Components.Builders;

public struct HtmlClassBuilder : IValueListBuilder<string, string>
{
    private const char SeparatorChar = ' ';

    private readonly StringBuilder _stringBuilder;

    public HtmlClassBuilder()
    {
        _stringBuilder = new StringBuilder();
    }

    public HtmlClassBuilder(string classes) : this()
    {
        if (classes.Length > 0)
        {
            _stringBuilder.Append(classes);

            if (classes.Last() != SeparatorChar)
            {
                _stringBuilder.Append(SeparatorChar);
            }
        }
    }

    public HtmlClassBuilder(HtmlClassBuilder other) : this(other.Build())
    {        
    }

    public IValueListBuilder<string, string> Add(string value)
    {
        Append(value);

        return this;
    }

    public IValueListBuilder<string, string> Add(string value, bool condition)
    {
        if (condition)
            Append(value);

        return this;
    }

    public IValueListBuilder<string, string> Add(string value, Func<bool> condition)
    {
        if (condition())
            Append(value);

        return this;
    }

    private void Append(string value)
    {
        _stringBuilder.Append($"{value}{SeparatorChar}");
    }

    public string Build()
    {
        return _stringBuilder
            .ToString()
            .TrimEnd();
    }
}

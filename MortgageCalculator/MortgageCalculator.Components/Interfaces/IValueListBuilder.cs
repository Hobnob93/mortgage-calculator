namespace MortgageCalculator.Components.Interfaces;

public interface IValueListBuilder<TValue, TBuild>
{
    IValueListBuilder<TValue, TBuild> Add(TValue value);
    IValueListBuilder<TValue, TBuild> Add(TValue value, bool condition);
    IValueListBuilder<TValue, TBuild> Add(TValue value, Func<bool> condition);

    TBuild Build();
}

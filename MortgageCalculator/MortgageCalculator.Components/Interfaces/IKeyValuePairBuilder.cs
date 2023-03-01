namespace MortgageCalculator.Components.Interfaces;

public interface IKeyValuePairBuilder<TKey, TValue, TBuild>
{
    IKeyValuePairBuilder<TKey, TValue, TBuild> Add(TKey key, TValue value);
    IKeyValuePairBuilder<TKey, TValue, TBuild> Add(TKey key, TValue value, bool condition);
    IKeyValuePairBuilder<TKey, TValue, TBuild> Add(TKey key, TValue value, Func<bool> condition);

    TBuild Build();
}

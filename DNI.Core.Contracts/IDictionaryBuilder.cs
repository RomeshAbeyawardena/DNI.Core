namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface IDictionaryBuilder<TKey, TValue>
    {
        IDictionary<TKey, TValue> ToDictionary();

        IDictionaryBuilder<TKey, TValue> Add(TKey key, TValue value);

        IDictionaryBuilder<TKey, TValue> Add(KeyValuePair<TKey, TValue> value);

        IDictionaryBuilder<TKey, TValue> AddRange(params KeyValuePair<TKey, TValue>[] value);

        IDictionaryBuilder<TKey, TValue> AddRange<T>(IEnumerable<T> value, Func<T, TKey> getKey, Func<T, TValue> getValue);
    }
}

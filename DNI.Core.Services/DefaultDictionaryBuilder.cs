namespace DNI.Core.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using DNI.Core.Contracts;

    internal sealed class DefaultDictionaryBuilder<TKey, TValue> : IDictionaryBuilder<TKey, TValue>
    {
        public static IDictionaryBuilder<TKey, TValue> Create()
        {
            return new DefaultDictionaryBuilder<TKey, TValue>(
                new ConcurrentDictionary<TKey, TValue>());
        }

        public static IDictionaryBuilder<TKey, TValue> Create(IDictionary<TKey, TValue> dictionary)
        {
            return new DefaultDictionaryBuilder<TKey, TValue>(
                new ConcurrentDictionary<TKey, TValue>(dictionary));
        }

        public IDictionaryBuilder<TKey, TValue> Add(TKey key, TValue value)
        {
            dictionary.TryAdd(key, value);
            return this;
        }

        public IDictionaryBuilder<TKey, TValue> Add(KeyValuePair<TKey, TValue> value)
        {
            Add(value.Key, value.Value);
            return this;
        }

        public IDictionaryBuilder<TKey, TValue> AddRange(params KeyValuePair<TKey, TValue>[] values)
        {
            foreach (var keyValuePair in values)
            {
                Add(keyValuePair);
            }

            return this;
        }

        public IDictionary<TKey, TValue> ToDictionary()
        {
            return new Dictionary<TKey, TValue>(dictionary);
        }

        public IDictionaryBuilder<TKey, TValue> AddRange<T>(IEnumerable<T> value, Func<T, TKey> getKey, Func<T, TValue> getValue)
        {
            return AddRange(
                value.Select(a => new KeyValuePair<TKey, TValue>(getKey(a), getValue(a)))
                .ToArray());
        }

        private DefaultDictionaryBuilder(IDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
        }

        private readonly IDictionary<TKey, TValue> dictionary;
    }
}

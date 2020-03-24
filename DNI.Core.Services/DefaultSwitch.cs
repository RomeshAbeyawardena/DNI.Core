namespace DNI.Core.Services
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using DNI.Core.Contracts;

    internal sealed class DefaultSwitch<TKey, TValue> : ISwitch<TKey, TValue>
    {
        private readonly IDictionary<TKey, TKey> alternateKeysDictionary;
        private readonly IDictionary<TKey, TValue> dictionary;

        private DefaultSwitch(IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TKey> alternateKeysDictionary)
        {
            this.dictionary = dictionary;
            this.alternateKeysDictionary = alternateKeysDictionary;
        }

        public IEnumerable<TKey> Keys => dictionary.Keys;

        public IEnumerable<TValue> Values => dictionary.Values;

        public int Count => dictionary.Count;

        public IReadOnlyDictionary<TKey, TKey> GetAlternativeKeysDictionary
            => new ReadOnlyDictionary<TKey, TKey>(alternateKeysDictionary);

        public TValue this[TKey key] => TryGetValue(key, out var value)
            ? value
            : default;

        public static ISwitch<TKey, TValue> Create()
        {
            return new DefaultSwitch<TKey, TValue>(
                new ConcurrentDictionary<TKey, TValue>(),
                new ConcurrentDictionary<TKey, TKey>());
        }

        public static ISwitch<TKey, TValue> Create(IDictionary<TKey, TValue> dictionary)
        {
            return new DefaultSwitch<TKey, TValue>(
                new ConcurrentDictionary<TKey, TValue>(dictionary),
                new ConcurrentDictionary<TKey, TKey>());
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public ISwitch<TKey, TValue> CaseWhen(TKey key, TValue value, params TKey[] alternateKeys)
        {
            if (ContainsKey(key)
                || !dictionary.TryAdd(key, value))
            {
                throw new ArgumentException($"Unable to add key: {key}", nameof(key));
            }

            if (alternateKeys != null
                && alternateKeys.Length > 0)
            {
                foreach (var alternateKey in alternateKeys)
                {
                    if (!alternateKeysDictionary.TryAdd(alternateKey, key))
                    {
                        throw new ArgumentException($"Unable to add alternate key: {key}", nameof(key));
                    }
                }
            }

            return this;
        }

        public TValue Case(TKey key)
        {
            if (dictionary.ContainsKey(key) && dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            if (alternateKeysDictionary.TryGetValue(key, out var primaryKey))
            {
                return Case(primaryKey);
            }

            return default;
        }
    }
}

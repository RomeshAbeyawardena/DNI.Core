using DNI.Core.Contracts;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DNI.Core.Services
{
    internal sealed class DefaultSwitch<TKey, TValue> : ISwitch<TKey, TValue>
    {
        public TValue this[TKey key] => TryGetValue(key, out var value) 
            ? value 
            : default;

        public IEnumerable<TKey> Keys => _dictionary.Keys;

        public IEnumerable<TValue> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public IReadOnlyDictionary<TKey, TKey> GetAlternativeKeysDictionary 
            => new ReadOnlyDictionary<TKey, TKey>(_alternateKeysDictionary);

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
            return _dictionary.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        private DefaultSwitch(IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TKey> alternateKeysDictionary)
        {
            _dictionary = dictionary;
            _alternateKeysDictionary = alternateKeysDictionary;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public ISwitch<TKey, TValue> CaseWhen(TKey key, TValue value, params TKey[] alternateKeys)
        {
            if(ContainsKey(key) 
                || !_dictionary.TryAdd(key, value))
                throw new ArgumentException($"Unable to add key: {key}", nameof(key));

            if(alternateKeys != null 
                && alternateKeys.Length > 0)
                foreach(var alternateKey in alternateKeys)
                    if(!_alternateKeysDictionary.TryAdd(alternateKey, key))
                        throw new ArgumentException($"Unable to add alternate key: {key}", nameof(key));

            return this;
        }

        public TValue Case(TKey key)
        {
            if(_dictionary.ContainsKey(key) && _dictionary.TryGetValue(key, out var value))
                return value;

            if(_alternateKeysDictionary.TryGetValue(key, out var primaryKey))
                return Case(primaryKey);

            return default;
        }

        private readonly IDictionary<TKey, TKey> _alternateKeysDictionary;
        private readonly IDictionary<TKey, TValue> _dictionary;
    }
}

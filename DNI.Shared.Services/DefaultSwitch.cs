using DNI.Shared.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal class DefaultSwitch<TKey, TValue> : ISwitch<TKey, TValue>
    {
        public TValue this[TKey key] => _dictionary[key];

        public IEnumerable<TKey> Keys => _dictionary.Keys;

        public IEnumerable<TValue> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public IReadOnlyDictionary<TKey, TKey> GetAlternativeKeysDictionary 
            => new ReadOnlyDictionary<TKey, TKey>(_alternateKeysDictionary);

        public static ISwitch<TKey, TValue> Create()
        {
            return new DefaultSwitch<TKey, TValue>();
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

        protected DefaultSwitch()
        {
            _dictionary = new Dictionary<TKey, TValue>();
            _alternateKeysDictionary = new Dictionary<TKey, TKey>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public ISwitch<TKey, TValue> CaseWhen(TKey key, TValue value, params TKey[] alternateKeys)
        {
            if(!ContainsKey(key))
                _dictionary.Add(key, value);

            if(alternateKeys != null 
                && alternateKeys.Length > 0)
                foreach(var alternateKey in alternateKeys)
                    _alternateKeysDictionary.Add(alternateKey, key);

            return this;
        }

        public TValue Case(TKey key)
        {
            if(_dictionary.ContainsKey(key))
                return this[key];

            if(_alternateKeysDictionary.TryGetValue(key, out var primaryKey))
                return Case(primaryKey);

            return default;
        }

        private IDictionary<TKey, TKey> _alternateKeysDictionary;
        private IDictionary<TKey, TValue> _dictionary;
    }
}

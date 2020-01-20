using DNI.Shared.Contracts;
using System.Collections.Generic;

namespace DNI.Shared.Services
{
    public static class DictionaryBuilder
    {
        public static IDictionaryBuilder<TKey,TValue> Create<TKey,TValue>()
        {
            return DefaultDictionaryBuilder<TKey, TValue>.Create();
        }

        public static IDictionaryBuilder<TKey,TValue> Create<TKey,TValue>(IDictionary<TKey, TValue> dictionary)
        {
            return DefaultDictionaryBuilder<TKey, TValue>.Create(dictionary);
        }
    }
}

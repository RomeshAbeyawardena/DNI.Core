using System.Collections.Generic;

namespace DNI.Core.Contracts
{
    public interface ISwitch<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        IReadOnlyDictionary<TKey, TKey> GetAlternativeKeysDictionary { get; }
        ISwitch<TKey, TValue> CaseWhen(TKey key, TValue value, params TKey[] alternateKeys);
        TValue Case(TKey key);
    }
}

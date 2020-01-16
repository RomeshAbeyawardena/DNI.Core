using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface ISwitch<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        IReadOnlyDictionary<TKey, TKey> GetAlternativeKeysDictionary { get; }
        ISwitch<TKey, TValue> CaseWhen(TKey key, TValue value, params TKey[] alternateKeys);
        TValue Case(TKey key);
    }
}

using DNI.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    public static class Switch
    {
        public static ISwitch<TKey, TValue> Create<TKey, TValue>()
        {
            return DefaultSwitch<TKey, TValue>.Create();
        }
    }
}

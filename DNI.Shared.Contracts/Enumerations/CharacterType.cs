using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Enumerations
{
    [Flags]
    public enum CharacterType
    {
        Uppercase = 0,
        Lowercase = 1,
        Numerics = 2,
        Symbols = 3
    }
}

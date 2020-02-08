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
        Uppercase = 1,
        Lowercase = 2,
        Numerics = 4,
        Symbols = 8
    }
}

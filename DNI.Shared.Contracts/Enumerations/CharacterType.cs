using System;

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

namespace DNI.Core.Contracts.Enumerations
{
    using System;

    /// <summary>
    /// Specifies a set of character types.
    /// </summary>
    [Flags]
    public enum CharacterType
    {
        Uppercase = 1,
        Lowercase = 2,
        Numerics = 4,
        Symbols = 8,
    }
}

using System;

namespace DNI.Core.Contracts.Enumerations
{
    [Flags]
    public enum OfType
    {
        DateTime = 0,
        Numeric = 1,
        Decimal = 2,
        String = 4
    }
}

namespace DNI.Core.Contracts.Enumerations
{
    using System;

    [Flags]
    public enum OfType
    {
        DateTime = 0,
        Numeric = 1,
        Decimal = 2,
        String = 4,
    }
}

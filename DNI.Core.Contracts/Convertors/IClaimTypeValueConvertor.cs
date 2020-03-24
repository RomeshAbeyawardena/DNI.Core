namespace DNI.Core.Contracts.Convertors
{
    using System;

    public interface IClaimTypeValueConvertor
    {
        string GetClaimTypeValue(Type type);

        object Convert(string value, string claimTypeValue);
    }
}

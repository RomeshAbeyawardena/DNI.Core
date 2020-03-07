using System;

namespace DNI.Core.Contracts.Convertors
{
    public interface IClaimTypeValueConvertor
    {
        string GetClaimTypeValue(Type type);
        object Convert(string value, string claimTypeValue);
    }
}

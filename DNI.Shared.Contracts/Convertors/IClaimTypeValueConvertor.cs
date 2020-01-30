using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Convertors
{
    public interface IClaimTypeValueConvertor
    {
        string GetClaimTypeValue(Type type);
        object Convert(string value, string claimTypeValue);
    }
}

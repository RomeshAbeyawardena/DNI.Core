using DNI.Core.Contracts.Enumerations;
using System;

namespace DNI.Core.Contracts
{
    public interface IIs
    {
        bool DateTimeOffSet(object value, out DateTimeOffset result);
        bool Numeric(object value, out long result);
        bool Decimal(object value, out decimal result);
        bool Bool(object value, out bool result);
        OfType TryDetermineType(object value, out dynamic determinedValue);
        bool Of<TType>(object value, Func<string, bool> isOfType, IConvertor<string, TType> convert, out TType result);

        bool Of<TType>(object value, Func<string, Tuple<bool,TType>> isOfType, out TType result);
    }
}

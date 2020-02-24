using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    internal sealed class DefaultIs : IIs
    {
        public bool Bool(object value, out bool result)
        {
            return Of(value, v => Tuple.Create(bool.TryParse(v, out var res), res), out result);
        }

        public bool Decimal(object value, out decimal result)
        {
            return Of(value, val => val.ToCharArray()
                .All(c => ContainsCharacters(c, true, true)) && val.Contains('.') , new DecimalConvertor(), out result);
        }

        public bool Numeric(object value, out long result)
        {
            return Of(value, val => val.ToCharArray()
                .All(c => ContainsCharacters(c, true, false)) , new NumericConvertor(), out result);
        }

        public bool Of<TType>(object value, Func<string, bool> isOfType, IConvertor<string, TType> convert, out TType result)
        {
            result = default;
            var valueString = value?.ToString();
            if(value == default || !isOfType(valueString))
                return false;

            return convert.TryConvert(valueString, out result);
        }

        private static bool ContainsCharacters(char character, bool isNumeric, bool isDecimal)
        {
            return (!isNumeric || char.IsNumber(character)) || (!isDecimal || character == '.' );
        }

        public bool DateTimeOffSet(object value, out DateTimeOffset result)
        {
            return Of(value, t => Tuple.Create(DateTimeOffset.TryParse(t, out var result1), result1), out result);
        }
            
        public bool Of<TType>(object value, Func<string, Tuple<bool,TType>> isOfType, out TType output)
        {
            output = default;

            if(value == null)
                return false;
            
            var result = isOfType(value.ToString());

            output = result.Item2;
            return result.Item1;
        }

        public OfType TryDetermineType(object value, out dynamic determinedValue)
        {
            if(Decimal(value, out var decimalValue))
            {
                determinedValue = decimalValue;
                return OfType.Decimal;
            }

            if(Numeric(value, out var longValue))
            {
                determinedValue = longValue;
                return OfType.Numeric;
            }

            if(DateTimeOffSet(value, out var dateTimeValue))
            {
                determinedValue = dateTimeValue;
                return OfType.DateTime;
            }

            determinedValue = value.ToString();
            return OfType.String;
        }

        private class DecimalConvertor : IConvertor<string, decimal>
        {
            public bool TryConvert(string source, out decimal result)
            {
                return decimal.TryParse(source, out result);
            }
        }

        private class NumericConvertor : IConvertor<string, long>
        {
            public bool TryConvert(string source, out long result)
            {
                return long.TryParse(source, out result);
            }
        }

    }
}

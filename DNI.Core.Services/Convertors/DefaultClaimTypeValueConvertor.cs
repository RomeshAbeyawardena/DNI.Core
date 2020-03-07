using DNI.Core.Contracts;
using DNI.Core.Contracts.Convertors;
using System;
using System.Security.Claims;

namespace DNI.Core.Services.Convertors
{
    public class DefaultClaimTypeValueConvertor : IClaimTypeValueConvertor
    {
        private readonly ISwitch<Type, string> _valueTypeDictionary;
        private readonly ISwitch<string, Func<string, object>> _valueTypeConvertor;

        public DefaultClaimTypeValueConvertor()
        {
            _valueTypeDictionary = Switch.Create<Type, string>()
                    .CaseWhen(typeof(short), ClaimValueTypes.Integer)
                    .CaseWhen(typeof(int), ClaimValueTypes.Integer32)
                    .CaseWhen(typeof(long), ClaimValueTypes.Integer64)
                    .CaseWhen(typeof(byte), ClaimValueTypes.UInteger32)
                    .CaseWhen(typeof(string), ClaimValueTypes.String)
                    .CaseWhen(typeof(Guid), ClaimValueTypes.KeyInfo)
                    .CaseWhen(typeof(decimal), ClaimValueTypes.Double)
                    .CaseWhen(typeof(DateTime), ClaimValueTypes.DateTime)
                    .CaseWhen(typeof(DateTimeOffset), ClaimValueTypes.DaytimeDuration);

            _valueTypeConvertor = Switch.Create<string, Func<string, object>>()
                .CaseWhen(ClaimValueTypes.String, value => value)
                .CaseWhen(ClaimValueTypes.Integer, value => short.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.Integer32, value => int.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.Integer64, value => long.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.UInteger32, value => uint.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.UInteger64, value => ulong.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.Boolean, value => bool.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.Double, value => decimal.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.DateTime, value => DateTime.TryParse(value, out var val) ? val : default, ClaimValueTypes.Date)
                .CaseWhen(ClaimValueTypes.KeyInfo, value => Guid.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.DaytimeDuration, value => DateTimeOffset.TryParse(value, out var val) ? val : default, ClaimValueTypes.Time);
        }

        public object Convert(string value, string claimTypeValue)
        {
            return _valueTypeConvertor.Case(claimTypeValue)?.Invoke(value);
        }

        public string GetClaimTypeValue(Type type)
        {
            return _valueTypeDictionary.Case(type);
        }
    }
}

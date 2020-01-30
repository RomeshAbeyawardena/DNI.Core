using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Services;
using DNI.Shared.Services.Attributes;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    public class JsonWebTokenService : IJsonWebTokenService
    {
        private readonly ISwitch<Type, string> _valueTypeDictionary;
        private readonly ISwitch<string, Func<string, object>> _valueTypeConvertor;
        private SecurityTokenDescriptor GetSecurityTokenDescriptor(SigningCredentials signingCredentials, 
            DateTime expiry, IDictionary<string, string> claimsDictionary)
        {
            var claims = claimsDictionary.Select((keyValuePair) => new Claim(keyValuePair.Key, keyValuePair.Value));
            return GetSecurityTokenDescriptor(signingCredentials, expiry, new ClaimsIdentity(claims));
        }

        private SecurityTokenDescriptor GetSecurityTokenDescriptor(SigningCredentials signingCredentials, 
            DateTime expiry, ClaimsIdentity claimsIdentity)
        {
            return new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = expiry,
                SigningCredentials = signingCredentials
            };
        }

        private SigningCredentials GetSigningCredentials(string secret, string securityAlgorithm, Encoding encoding)
        {
            var securityKey = new SymmetricSecurityKey(encoding.GetBytes(secret));
            return new SigningCredentials(securityKey, securityAlgorithm);
        }

        private Claim CreateClaim<T>(string claimType, T value)
        {
            var type = typeof(T);

            return new Claim(claimType, value.ToString(), _valueTypeDictionary.Case(type));
        }

       
        public JsonWebTokenService()
        {
               _valueTypeDictionary = Switch.Create<Type, string>()
                    .CaseWhen(typeof(short), ClaimValueTypes.Integer)
                    .CaseWhen(typeof(int), ClaimValueTypes.Integer32)
                    .CaseWhen(typeof(long), ClaimValueTypes.Integer64)
                    .CaseWhen(typeof(byte), ClaimValueTypes.UInteger32)
                    .CaseWhen(typeof(string), ClaimValueTypes.String)
                    .CaseWhen(typeof(Guid), ClaimValueTypes.Base64Binary)
                    .CaseWhen(typeof(decimal), ClaimValueTypes.Double)
                    .CaseWhen(typeof(DateTime), ClaimValueTypes.DateTime)
                    .CaseWhen(typeof(DateTimeOffset), ClaimValueTypes.DaytimeDuration);

            _valueTypeConvertor = Switch.Create<string, Func<string, object>>()
                .CaseWhen(ClaimValueTypes.Integer, value => short.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.Integer32, value => int.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.Integer64, value => long.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.Boolean, value => bool.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.Double, value => decimal.TryParse(value, out var val) ? val : default)
                .CaseWhen(ClaimValueTypes.DateTime, value => DateTime.TryParse(value, out var val) ? val : default);

        }

        public string CreateToken(DateTime expiry, IDictionary<string, string> claimsDictionary, string secret, Encoding encoding)
        {
            if(encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            var tokenHandler = new JwtSecurityTokenHandler();
            var signingCredentials = GetSigningCredentials(secret, SecurityAlgorithms.HmacSha512Signature, encoding);
            var token = tokenHandler.CreateToken(GetSecurityTokenDescriptor(signingCredentials, expiry, claimsDictionary));
            return tokenHandler.WriteToken(token);
        }

        public IDictionary<string, string> ParseToken(string token, string secret, Encoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            var tokenHandler = new JwtSecurityTokenHandler();
            var signingCredentials = GetSigningCredentials(secret, SecurityAlgorithms.HmacSha512Signature, encoding);
            var securityToken = tokenHandler.ReadJwtToken(token);
            return securityToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
        }

        public ClaimsIdentity GetClaimsIdentity<T>(T value)
        {
            var type = typeof(T);
            var claimProperties = GetClaimProperties(type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance));

            return new ClaimsIdentity(claimProperties
                .Select(claimProperty => { 
                    var claimAttribute = claimProperty.GetCustomAttribute<ClaimAttribute>();
                    var propertyValue = claimProperty.GetValue(value); 
                    return CreateClaim(claimAttribute?.ClaimType ?? claimProperty.Name, propertyValue);  
                }));
        }

        public T ParseClaims<T>(IEnumerable<Claim> claims, params object[] tArgs)
        {
            var type = typeof(T);
            var claimProperties = GetClaimProperties(type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance));
            var instance = Activator.CreateInstance(typeof(T), tArgs);
            foreach(var claim in claims)
            {
                var property = claimProperties.FirstOrDefault(property => { 
                    var claimAttribute = property.GetCustomAttribute<ClaimAttribute>(); 
                    return claimAttribute.ClaimType == claim.Type || property.Name == claim.Type; 
                });
                
                if(property == null)
                    continue;

                property.SetValue(instance, Convert.ChangeType(claim.Value, property.PropertyType));
            }

            return (T)instance;
        }

        private IEnumerable<PropertyInfo> GetClaimProperties(IEnumerable<PropertyInfo> properties)
        {
            return properties.Where(property => property.GetCustomAttribute<ClaimAttribute>() != null);
        }

        public string CreateToken<T>(DateTime expiry, T claims, string secret, Encoding encoding)
        {
            
            if(encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            var tokenHandler = new JwtSecurityTokenHandler();
            var signingCredentials = GetSigningCredentials(secret, SecurityAlgorithms.HmacSha512Signature, encoding);
            var token = tokenHandler.CreateToken(GetSecurityTokenDescriptor(signingCredentials, expiry, GetClaimsIdentity(claims)));
            return tokenHandler.WriteToken(token);
        }
    }
}

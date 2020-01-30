using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Convertors;
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
        private readonly IClaimTypeValueConvertor _claimTypeValueConvertor;

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

        private Claim CreateClaim(string claimType, object value, Type type)
        {
            return new Claim(claimType, value.ToString(), _claimTypeValueConvertor.GetClaimTypeValue(type));
        }

       
        public JsonWebTokenService(IClaimTypeValueConvertor claimTypeValueConvertor)
        {
               _claimTypeValueConvertor = claimTypeValueConvertor;
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
                    return CreateClaim(claimAttribute?.ClaimType ?? claimProperty.Name, propertyValue, claimProperty.PropertyType);  
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

                property.SetValue(instance, _claimTypeValueConvertor.Convert(claim.Value, claim.ValueType));
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

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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;


namespace DNI.Shared.Services
{
    public class JsonWebTokenService : IJsonWebTokenService
    {
        private readonly IClaimTypeValueConvertor _claimTypeValueConvertor;

        private SecurityTokenDescriptor GetSecurityTokenDescriptor(string issuer, string audience, SigningCredentials signingCredentials, 
            DateTime expiry, IDictionary<string, string> claimsDictionary)
        {
            var claims = claimsDictionary.Select((keyValuePair) => new Claim(keyValuePair.Key, keyValuePair.Value));
            return GetSecurityTokenDescriptor(issuer, audience, signingCredentials, expiry, new ClaimsIdentity(claims));
        }

        private SecurityTokenDescriptor GetSecurityTokenDescriptor(string issuer, string audience, SigningCredentials signingCredentials, 
            DateTime expiry, ClaimsIdentity claimsIdentity)
        {
            
            return new SecurityTokenDescriptor
            {
                Issuer = issuer, //"app.test.branch.local",
                Audience = audience, //"test.branch.local",
                Subject = claimsIdentity,
                Expires = expiry,
                SigningCredentials = signingCredentials,
            };
        }

        private SigningCredentials GetSigningCredentials(string secret, string securityAlgorithm, Encoding encoding)
        {
            var securityKey = new SymmetricSecurityKey(encoding.GetBytes(secret));
            var signinCredentials = new SigningCredentials(securityKey, securityAlgorithm, SecurityAlgorithms.Sha512Digest);   
            
            Console.WriteLine(signinCredentials.Digest);
            Console.WriteLine(signinCredentials.Kid);
            Console.WriteLine(signinCredentials.Key);

            return signinCredentials;
        }

        private Claim CreateClaim(string claimType, object value, Type type)
        {
            return new Claim(claimType, value.ToString(), _claimTypeValueConvertor.GetClaimTypeValue(type));
        }

       
        public JsonWebTokenService(IClaimTypeValueConvertor claimTypeValueConvertor)
        {
               _claimTypeValueConvertor = claimTypeValueConvertor;
        }

        public string CreateToken(string issuer, string audience, DateTime expiry, IDictionary<string, string> claimsDictionary, string secret, Encoding encoding)
        {
            if(encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            var tokenHandler = new JwtSecurityTokenHandler();
            var signingCredentials = GetSigningCredentials(secret, SecurityAlgorithms.HmacSha512Signature, encoding);
            var token = tokenHandler.CreateToken(GetSecurityTokenDescriptor(issuer, audience, signingCredentials, expiry, claimsDictionary));
            return tokenHandler.WriteToken(token);
        }

        public bool TryParseToken(string token, string secret, Encoding encoding, TokenValidationParameters tokenValidationParameters, out IDictionary<string, string> claims)
        {
            var handledExceptions = new [] { 
                typeof(SecurityTokenInvalidAudienceException), 
                typeof(SecurityTokenInvalidSigningKeyException),
                typeof(SecurityTokenInvalidSignatureException),
                typeof(ArgumentException)
            };

            try
            { 
                if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

                var tokenHandler = new JwtSecurityTokenHandler();

                var signingCredentials = GetSigningCredentials(secret, SecurityAlgorithms.HmacSha512Signature, encoding);

                tokenValidationParameters.IssuerSigningKey = signingCredentials.Key;
                
                var securityClaimPrinciple = tokenHandler.ValidateToken(token, tokenValidationParameters, out var m);
            
                var securityToken = tokenHandler.ReadJwtToken(token);
                    claims = securityToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
                
                return true;
            }
            catch (Exception ex)
            {
                if(!handledExceptions.Contains(ex.GetType()))
                    throw;

                Console.WriteLine(ex);

                claims = null;
                return false;
            }
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
    }
}

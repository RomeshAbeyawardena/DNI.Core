using DNI.Shared.Contracts.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
{
    public class JsonWebTokenService : IJsonWebTokenService
    {
        private SecurityTokenDescriptor GetSecurityTokenDescriptor(SigningCredentials signingCredentials, 
            DateTime expiry, IDictionary<string, string> claimsDictionary)
        {
            var claims = claimsDictionary.Select((keyValuePair) => new Claim(keyValuePair.Key, keyValuePair.Value));
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiry,
                SigningCredentials = signingCredentials
            };
        }

        private SigningCredentials GetSigningCredentials(string secret, string securityAlgorithm, Encoding encoding)
        {
            var securityKey = new SymmetricSecurityKey(encoding.GetBytes(secret));
            return new SigningCredentials(securityKey, securityAlgorithm);
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
    }
}

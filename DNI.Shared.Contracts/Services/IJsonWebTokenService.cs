using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DNI.Shared.Contracts.Services
{
    public interface IJsonWebTokenService
    {
        string CreateToken(Action<SecurityTokenDescriptor> populateSecurityTokenDescriptor, DateTime expiry, 
            IDictionary<string, string> claimsDictionary, string secret, Encoding encoding);
        bool TryParseToken(string token, string secret, 
            Action<TokenValidationParameters> populateTokenValidationParameters, 
            Encoding encoding, out IDictionary<string, string> claims);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace DNI.Shared.Contracts.Services
{
    public interface IJsonWebTokenService
    {
        ClaimsIdentity GetClaimsIdentity<T>(T value);
        string CreateToken(string issuer, string audience, DateTime expiry, IDictionary<string, string> claimsDictionary, string secret, Encoding encoding);
        bool TryParseToken(string token, string secret, Encoding encoding, TokenValidationParameters tokenValidationParameters, out IDictionary<string, string> claims);
    }
}

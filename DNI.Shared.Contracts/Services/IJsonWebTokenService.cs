using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Services
{
    public interface IJsonWebTokenService
    {
        ClaimsIdentity GetClaimsIdentity<T>(T value);
        string CreateToken(DateTime expiry, IDictionary<string, string> claimsDictionary, string secret, Encoding encoding);
        string CreateToken<T>(DateTime expiry, T claims, string secret, Encoding encoding);
        IDictionary<string,string> ParseToken(string token, string secret, Encoding encoding);
        T ParseClaims<T>(IEnumerable<Claim> claims, params object[] tArgs);
    }
}

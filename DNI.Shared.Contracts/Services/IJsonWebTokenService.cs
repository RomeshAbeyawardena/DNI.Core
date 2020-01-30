using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Services
{
    public interface IJsonWebTokenService
    {
        string CreateToken(DateTime expiry, IDictionary<string, string> claimsDictionary, string secret, Encoding encoding);
        IDictionary<string,string> ParseToken(string token, string secret, Encoding encoding);
    }
}

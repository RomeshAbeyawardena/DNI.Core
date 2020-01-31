using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Providers;
using DNI.Shared.Domains;
using System;
using DNI.Shared.Shared.Extensions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNI.Shared.App.Domains;
using Microsoft.Extensions.Logging;
using DNI.Shared.Contracts.Services;
using DNI.Shared.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;

namespace DNI.Shared.App
{
    public class Startup
    {
        private readonly IJsonWebTokenService _jsonTokenService;
        private readonly ILogger<Startup> _logger;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ICryptographicCredentials _cryptographicCredentials;
        private readonly IHashingProvider _hashingProvider;
        private readonly ICryptographyProvider _cryptographyProvider;

        public async Task<int> Begin(params object[] args)
        {
            IdentityModelEventSource.ShowPII = true;

            var mySecret = "cc7830e8cb754617a00eb1f068733f0cb85b12cb-e09f-455a-b4c2";
            var mySecret2 = "cc7830e8cb754617a00eb1f068733f0cb85b12cb-e09f-455a-b4c2-68ad24f4";

            var issuer = "http://master.test.branch.local";
            var audience = "http://app.test.branch.local";
            var audience2 = "http://app1.test.branch.local";
            var userSession = _jsonTokenService.CreateToken(parameters => {
                parameters.Issuer = issuer;
                parameters.Audience = audience;
            }, DateTime.Now.AddSeconds(1), DictionaryBuilder
                .Create<string, string>(builder => builder
                    .Add("BusinessUnitId", "23829")
                    .Add("RoleId", "1234"))
                .ToDictionary(), mySecret2, Encoding.UTF8);


            if (_jsonTokenService.TryParseToken(userSession, mySecret2, parameters => { 
                parameters.ValidIssuer = issuer; 
                parameters.ValidAudience = audience; }, 
                Encoding.UTF8, out var claimsDictionary))
            {
                Console.WriteLine(claimsDictionary.Count());
            }

            return 0;
        }

        public Startup(ILogger<Startup> logger, IJsonWebTokenService jsonTokenService, IRepository<Customer> customerRepository, ICryptographicCredentials cryptographicCredentials, IHashingProvider hashingProvider, 
            ICryptographyProvider cryptographyProvider)
        {
            _jsonTokenService = jsonTokenService;
            _logger = logger;
            _customerRepository = customerRepository;
            _cryptographicCredentials = cryptographicCredentials;
            _hashingProvider = hashingProvider;
            _cryptographyProvider = cryptographyProvider;
        }
    }
}

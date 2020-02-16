using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Providers;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNI.Shared.App.Domains;
using Microsoft.Extensions.Logging;
using DNI.Shared.Shared.Extensions;
using DNI.Shared.Contracts.Services;
using DNI.Shared.Services;
using Microsoft.IdentityModel.Logging;
using MessagePack;
using DNI.Shared.Services.Options;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using DNI.Shared.Contracts.Generators;
using DNI.Shared.Contracts.Enumerations;
using System.Diagnostics;
using DNI.Shared.Domains;

namespace DNI.Shared.App
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEncryptionProvider _encryptionProvider;
        private readonly IHashingProvider _hashingProvider;
        private readonly ISwitch<string, ICryptographicCredentials> _credentialsDictionary;
        private readonly IRandomStringGenerator _randomStringGenerator;

        public async Task<int> Begin(params object[] args)
        {
            var result = Response.Success<CustomerResponse>(new Customer { Id = 1 });
            return 0;
        }

        public class CustomerResponse : ResponseBase<Customer> { }


        public Startup(ILogger<Startup> logger, IEncryptionProvider encryptionProvider,
            ISwitch<string, ICryptographicCredentials> credentialsDictionary,
            IHashingProvider hashingProvider, IHttpClientFactory httpClientFactory,
            IRandomStringGenerator randomStringGenerator)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _encryptionProvider = encryptionProvider;
            _hashingProvider = hashingProvider;
            _credentialsDictionary = credentialsDictionary;
            _randomStringGenerator = randomStringGenerator;
        }
    }
}

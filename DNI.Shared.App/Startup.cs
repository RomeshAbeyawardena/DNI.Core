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

namespace DNI.Shared.App
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEncryptionProvider _encryptionProvider;
        private readonly IHashingProvider _hashingProvider;
        private readonly ISwitch<string, ICryptographicCredentials> _credentialsDictionary;

        public async Task<int> Begin(params object[] args)
        {
            var httpClient = _httpClientFactory.GetHttpClient("myEndPoint", "http://www.google.com", (request) => {
                request.Headers.Add("bot-id", long.MaxValue.ToString());
            });

            var response = await httpClient.GetAsync("/maps");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine(content);

            httpClient = _httpClientFactory.GetHttpClient("myEndPoint2", "http://www.bing.com", (request) => {
                request.Headers.Add("bot-id", long.MaxValue.ToString());
            });

            response = await httpClient.GetAsync("/maps");
            var customer = await response.Content.ToObject<CustomerDto>();

            Console.WriteLine(content);

            return 0;
        }

        public Startup(ILogger<Startup> logger, IEncryptionProvider encryptionProvider, 
            ISwitch<string, ICryptographicCredentials> credentialsDictionary, 
            IHashingProvider hashingProvider, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _encryptionProvider = encryptionProvider;
            _hashingProvider = hashingProvider;
            _credentialsDictionary = credentialsDictionary;
        }
    }
}

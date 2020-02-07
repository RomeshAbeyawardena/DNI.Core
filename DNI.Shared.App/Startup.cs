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
            var dictionary = new Dictionary<string, string>();

            dictionary.Add("UniqueReference", "5e8de8d4-4da2-457f-8ef2-10bb962f5939");
            dictionary.Add("Name", "John");
            dictionary.Add("Code", "JD00001");
            dictionary.Add("Age", "33");
            dictionary.Add("Height", "1.524");
            dictionary.Add("Weight", "333.43");
            dictionary.Add("DateOfBirth", "11/01/1986 13:00");
            dictionary.Add("IsActive", "1");
            dictionary.Add("Reference", "42DJ13948211");

            var data = dictionary.ToClaimObject<Data>();

            Console.WriteLine(data.UniqueReference);
            Console.WriteLine(data.Name);
            Console.WriteLine(data.Code);
            Console.WriteLine(data.Height);
            Console.WriteLine(data.Age);
            Console.WriteLine(data.Weight);
            Console.WriteLine(data.DateOfBirth);
            Console.WriteLine(data.IsActive);
            Console.WriteLine(data.CustomerReference);
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

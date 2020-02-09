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
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                var stopWatch = Stopwatch.StartNew();
                var generatedString = _randomStringGenerator.GenerateString(
                    CharacterType.Lowercase | CharacterType.Uppercase | CharacterType.Numerics | CharacterType.Symbols, 16);
                Console.WriteLine("{0} ({1})", generatedString, generatedString.Length);
                stopWatch.Stop();

                stopWatch = Stopwatch.StartNew();
                generatedString = _randomStringGenerator.GenerateString(
                    CharacterType.Lowercase | CharacterType.Uppercase | CharacterType.Numerics | CharacterType.Symbols, 32);
                Console.WriteLine("{0} ({1})", generatedString, generatedString.Length);
                stopWatch.Stop();

                Console.WriteLine(stopWatch.Elapsed);

                var stopWatch2 = Stopwatch.StartNew();
                generatedString = _randomStringGenerator.GenerateString(
                    CharacterType.Lowercase | CharacterType.Uppercase | CharacterType.Numerics, 64);
                Console.WriteLine("{0} ({1})", generatedString, generatedString.Length);
                stopWatch2.Stop();

                Console.WriteLine(stopWatch.Elapsed);
            }
            return 0;
        }

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

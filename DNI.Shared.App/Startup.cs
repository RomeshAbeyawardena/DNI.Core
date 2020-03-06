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
using System.Reactive.Subjects;
using Microsoft.IO;
using System.IO;

namespace DNI.Shared.App
{
    public class Startup
    {
        private readonly IIs _is;
        private readonly ILogger<Startup> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly ISubject<RecyclableMemoryStreamManagerState> _subject;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEncryptionProvider _encryptionProvider;
        private readonly IHashingProvider _hashingProvider;
        private readonly ISwitch<string, ICryptographicCredentials> _credentialsDictionary;
        private readonly IRandomStringGenerator _randomStringGenerator;

        public async Task<int> Begin(params object[] args)
        {
            using (var memoryStream = _recyclableMemoryStreamManager.GetStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream)){
                    await streamWriter.WriteLineAsync("Lorem ipsum");
                }
            }

            using (var memoryStream = _recyclableMemoryStreamManager.GetStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                    await streamWriter.WriteLineAsync("Lorem ipsum");
            }

            return 0;
        }

        public class CustomerResponse : ResponseBase<Customer> { }


        public Startup(ILogger<Startup> logger, 
            RecyclableMemoryStreamManager recyclableMemoryStreamManager,
            ISubject<RecyclableMemoryStreamManagerState> subject)
        {
            _logger = logger;
            _recyclableMemoryStreamManager = recyclableMemoryStreamManager;
            _subject = subject;
            _subject.Subscribe(onNext, onError, onCompleted);
        }

        private void onCompleted()
        {
            throw new NotImplementedException();
        }

        private void onError(Exception obj)
        {
            throw new NotImplementedException();
        }

        private void onNext(RecyclableMemoryStreamManagerState obj)
        {
            if(obj.StreamCreated)
                _logger.LogInformation("Stream created");

            if(obj.BlockCreated)
                _logger.LogInformation("Block created");

            if(obj.BlockDiscarded)
                _logger.LogInformation("Block dsicarded");

            if(obj.StreamDisposed)
                _logger.LogInformation("Stream disposed");

            if(obj.StreamFinalized)
                _logger.LogInformation("Stream finalized");

            if(obj.UsageReportRequested)
                _logger.LogInformation("Large Pool\r\n\tFree: {0} bytes\r\n\tIn Use: {1}\r\n" 
                    + "Small Pool\r\n\tFree: {2}\r\n\tIn Use: {3}", 
                    obj.LargePoolFreeBytes, 
                    obj.LargePoolInUseBytes, obj.SmallPoolFreeBytes,
                    obj.SmallPoolInUseBytes);
        }
    }
}

using System;
using System.Threading.Tasks;
using DNI.Core.App.Domains;
using Microsoft.Extensions.Logging;
using DNI.Core.Domains;
using System.Reactive.Subjects;
using Microsoft.IO;
using System.IO;
using DNI.Core.Domains.States;

namespace DNI.Core.App
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly ISubject<RecyclableMemoryStreamManagerState> _subject;

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
            RecyclableMemoryStreamManagerState.Handle(obj, 
                () => _logger.LogInformation("Stream created"),
                () => _logger.LogInformation("Block created"),
                () => _logger.LogInformation("Block discarded"),
                () => _logger.LogInformation("Stream disposed"),
                () => _logger.LogInformation("Stream finalized"),
                onUsageReportRequested: (largePoolFreeBytes, largePoolInUseBytes,
                 smallPoolFreeBytes, smallPoolInUseBytes) => _logger.LogInformation("Large Pool\r\n\tFree: {0} bytes\r\n\tIn Use: {1}\r\n" 
                    + "Small Pool\r\n\tFree: {2}\r\n\tIn Use: {3}", 
                    largePoolFreeBytes, 
                    largePoolInUseBytes, smallPoolFreeBytes,
                    smallPoolInUseBytes));
        }
    }
}

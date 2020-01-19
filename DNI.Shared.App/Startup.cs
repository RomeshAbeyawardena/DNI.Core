using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.App
{
    public class Startup
    {
        private readonly ICryptographicCredentials _cryptographicCredentials;
        private readonly IHashingProvider _hashingProvider;
        private readonly ICryptographyProvider _cryptographyProvider;

        public async Task<int> Begin(params object[] args)
        {

            var firstRun = true;
            ConsoleKeyInfo lastConsoleKeyInfo = default;
            while (firstRun || (lastConsoleKeyInfo = Console.ReadKey()).Key != ConsoleKey.Escape)
            {
                if (lastConsoleKeyInfo != default)
                    Console.Write(lastConsoleKeyInfo.KeyChar);

                firstRun = false;
                Console.Write("\r\nValue to encrypt: ");
                var encryptedValue = _cryptographyProvider.Encrypt(_cryptographicCredentials, Console.ReadLine());
                var decryptedValue = _cryptographyProvider.Decrypt(_cryptographicCredentials, await encryptedValue);
                Console.WriteLine("You entered: {0}", await decryptedValue);
                Console.WriteLine("Press any key to continue, press Escape to quit.");
            }

            return 0;
        }

        public Startup(ICryptographicCredentials cryptographicCredentials, IHashingProvider hashingProvider, 
            ICryptographyProvider cryptographyProvider)
        {
            _cryptographicCredentials = cryptographicCredentials;
            _hashingProvider = hashingProvider;
            _cryptographyProvider = cryptographyProvider;
        }
    }
}

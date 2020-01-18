using DNI.Shared.Services;
using DNI.Shared.Services.Abstraction;
using DNI.Shared.Services.Extensions;
using DNI.Shared.Services.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;
using DNI.Shared.Shared.Extensions;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using DNI.Shared.Contracts;
using System.Collections.Generic;

namespace DNI.Shared.App
{
    public static class Program
    {
        public static async Task Main()
        {
            var hashingProvider = new HashingProvider();

            var crypto = new CryptographyProvider(hashingProvider);

            var cryptoCredentials = crypto.GetCryptographicCredentials<MCryptographicCredentials>(KeyDerivationPrf.HMACSHA512, "myPassword123456", "MySecureSalt1234567890101".GetBytes(Encoding.ASCII), 1000000, 32, null);

            var firstRun = true;
            ConsoleKeyInfo lastConsoleKeyInfo = default;
            while(firstRun || (lastConsoleKeyInfo = Console.ReadKey()).Key != ConsoleKey.Escape)
            { 
                if(lastConsoleKeyInfo != default)
                    Console.Write(lastConsoleKeyInfo.KeyChar);

                firstRun = false;
                Console.Write("\r\nValue to encrypt: ");
                var encryptedValue = crypto.Encrypt(cryptoCredentials, Console.ReadLine());
                var decryptedValue = crypto.Decrypt(cryptoCredentials, await encryptedValue);
                Console.WriteLine("You entered: {0}", await decryptedValue);
            }
            
        }

        public static void OnCatch(Exception ex)
        {
            Console.WriteLine(ex);
        }

        class MCryptographicCredentials : ICryptographicCredentials
        {
            public IEnumerable<byte> Key { get; set; }
            public IEnumerable<byte> InitialVector { get; set; }
            public string SymmetricAlgorithm { get; set; }
        }


        class MClass
        {
            public int K1 { get; set; }
            public string V1 { get; set; }
        }

        public class ServiceBroker : ServiceBrokerBase
        {
            public ServiceBroker()
            {
                Assemblies = new [] { DefaultAssembly, GetAssembly<ServiceBroker>() };
            }
        }

        public class MyDisposable : IDisposable
        {
            public string Name { get; }

            public void Dispose()
            {
                Dispose(true);
            }

            public int GetNumber()
            {
                return 5;
            }

            public async Task<int> GetNumberAsync()
            {
                return await Task.FromResult(GetNumber());
            }

            public void CallMe()
            {
                Console.WriteLine($"{ Name } CallMe was invoked.");
            }

            public async Task CallMeAsync()
            {
                await Task.Delay(1000);
                Console.WriteLine($"{ Name } CallMe was invoked async.");
            }

            protected virtual void Dispose(bool gc)
            {
                if(gc)
                    Console.WriteLine($"Then { Name } was disposed");
            }

            public MyDisposable(string name)
            {
                Name = name;
            }
        }
    }
}

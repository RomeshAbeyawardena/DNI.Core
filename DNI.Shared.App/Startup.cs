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

namespace DNI.Shared.App
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ICryptographicCredentials _cryptographicCredentials;
        private readonly IHashingProvider _hashingProvider;
        private readonly ICryptographyProvider _cryptographyProvider;

        public async Task<int> Begin(params object[] args)
        {
            //Console.Write("Enter Password:");
            //var password = _hashingProvider.HashBytes(Constants.SHA512, Console.ReadLine()
            //    .GetBytes(Encoding.ASCII));

            //Console.Write("Confirm Password:");
            //var confirmPassword = _hashingProvider.HashBytes(Constants.SHA512, Console.ReadLine()
            //    .GetBytes(Encoding.ASCII));

            //if(password.SequenceEqual(confirmPassword))
            //    Console.WriteLine("Password's match!");
            // {E23C96F2-30B4-4890-BB5D-E57E7AADA982}
            
            var customer = new Customer { FirstName = "Sam", MiddleName = "Smith", LastName = "McDonald" };
            _logger.LogInformation("{0}", customer);
            await _customerRepository.SaveChanges(customer, false);

            customer.Id = 1;
            customer.MiddleName = "Mantaz";

            customer = await _customerRepository.SaveChanges(customer, false);


            //var firstRun = true;
            //ConsoleKeyInfo lastConsoleKeyInfo = default;
            //while (firstRun || (lastConsoleKeyInfo = Console.ReadKey()).Key != ConsoleKey.Escape)
            //{
            //    if (lastConsoleKeyInfo != default)
            //        Console.Write(lastConsoleKeyInfo.KeyChar);

            //    firstRun = false;
            //    Console.Write("\r\nValue to encrypt: ");
            //    var encryptedValue = _cryptographyProvider.Encrypt(_cryptographicCredentials, Console.ReadLine());
            //    var decryptedValue = _cryptographyProvider.Decrypt(_cryptographicCredentials, await encryptedValue);
            //    Console.WriteLine("You entered: {0}", await decryptedValue);
            //    Console.WriteLine("Press any key to continue, press Escape to quit.");
            //}

            return 0;
        }

        public Startup(ILogger<Startup> logger, IRepository<Customer> customerRepository, ICryptographicCredentials cryptographicCredentials, IHashingProvider hashingProvider, 
            ICryptographyProvider cryptographyProvider)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _cryptographicCredentials = cryptographicCredentials;
            _hashingProvider = hashingProvider;
            _cryptographyProvider = cryptographyProvider;
        }
    }
}

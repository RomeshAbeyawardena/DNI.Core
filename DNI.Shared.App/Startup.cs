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

namespace DNI.Shared.App
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly IEncryptionProvider _encryptionProvider;
        private readonly IHashingProvider _hashingProvider;
        private readonly ISwitch<string, ICryptographicCredentials> _credentialsDictionary;

        public async Task<int> Begin(params object[] args)
        {
            var customer = new CustomerDto
            {
                Created = DateTime.Now,
                Modified = DateTime.Now,
                EmailAddress = "jane.doe@hotmail.com",
                FirstName = "Jane",
                MiddleName = "Middleton",
                LastName = "Doe",
                UniqueId = Guid.NewGuid(),
                Password = "myP@ssw0rd1!".GetBytes(Encoding.UTF8).ToArray(),
                Id = 1
            };
            _logger.LogInformation("test");
            var encrypted = await _encryptionProvider.Encrypt<CustomerDto, Customer>(customer);

            if(!_credentialsDictionary.TryGetValue(Constants.PersonalDataEncryption, out var defaultCredentials))
                throw new NullReferenceException();

            var passwordHash = _hashingProvider.PasswordDerivedBytes("myP@ssw0rd1!", defaultCredentials.Key, defaultCredentials.KeyDerivationPrf, defaultCredentials.Iterations, defaultCredentials.TotalNumberOfBytes);
            var passwordHash2 = _hashingProvider.PasswordDerivedBytes("myP@ssw0rd1!", defaultCredentials.Key, defaultCredentials.KeyDerivationPrf, defaultCredentials.Iterations, defaultCredentials.TotalNumberOfBytes);

            if (!passwordHash2.SequenceEqual(passwordHash.ToArray()))
                throw new UnauthorizedAccessException();

            if (!encrypted.Password.SequenceEqual(passwordHash.ToArray()))
                throw new UnauthorizedAccessException();

            var decrypted = await _encryptionProvider.Decrypt<Customer, CustomerDto>(encrypted);
            return 0;
        }

        public Startup(ILogger<Startup> logger, IEncryptionProvider encryptionProvider, 
            ISwitch<string, ICryptographicCredentials> credentialsDictionary, 
            IHashingProvider hashingProvider)
        {
            _logger = logger;
            _encryptionProvider = encryptionProvider;
            _hashingProvider = hashingProvider;
            _credentialsDictionary = credentialsDictionary;
        }
    }
}

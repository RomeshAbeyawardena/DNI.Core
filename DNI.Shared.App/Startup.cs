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
                Password = "myP@ssw0rd1!".GetBytes(Encoding.UTF8),
                Id = 1
            };

            var encrypted = await _encryptionProvider.Encrypt<CustomerDto, Customer>(customer);
            var decrypted = await _encryptionProvider.Decrypt<Customer, CustomerDto>(encrypted);
            return 0;
        }

        public Startup(ILogger<Startup> logger, IEncryptionProvider encryptionProvider)
        {
            _logger = logger;
            _encryptionProvider = encryptionProvider;
        }
    }
}

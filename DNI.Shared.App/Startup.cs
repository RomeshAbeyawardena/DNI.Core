using DNI.Shared.Contracts;
using DNI.Shared.Contracts.Providers;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNI.Shared.App.Domains;
using Microsoft.Extensions.Logging;
using DNI.Shared.Contracts.Services;
using DNI.Shared.Services;
using Microsoft.IdentityModel.Logging;
using MessagePack;
using DNI.Shared.Services.Options;

namespace DNI.Shared.App
{
    public class Startup
    {
        private readonly IJsonWebTokenService _jsonTokenService;
        private readonly ILogger<Startup> _logger;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ICryptographicCredentials _cryptographicCredentials;
        private readonly IHashingProvider _hashingProvider;
        private readonly ICryptographyProvider _cryptographyProvider;
        private readonly IMessagePackService _messagePackService;

        public async Task<int> Begin(params object[] args)
        {
            var queryableList = ListBuilder.Create<Customer>();

            for(var index = 0; index < 142; index++)
                queryableList.Add(new Customer { Id = index + 1 });

            var query = queryableList.ToList().AsQueryable();

            var pager = DefaultPagerResult.Create(query);
            
            
            var list = await pager.GetItems(1, 10, false);
            var list2 = await pager.GetItems(2, 10, false);
            var lis3 = await pager.GetItems(3, 10, false);
            var lis12 = await pager.GetItems(12, 10, false);
            var lis13 = await pager.GetItems(13, 10, false);
            var lis14 = await pager.GetItems(14, 10, false);
            var lis15 = await pager.GetItems(15, 10, false);
            return 0;
        }

        public Startup(ILogger<Startup> logger, IJsonWebTokenService jsonTokenService, IRepository<Customer> customerRepository, ICryptographicCredentials cryptographicCredentials, IHashingProvider hashingProvider, IMessagePackService messagePackService,
            ICryptographyProvider cryptographyProvider)
        {
            _jsonTokenService = jsonTokenService;
            _logger = logger;
            _customerRepository = customerRepository;
            _cryptographicCredentials = cryptographicCredentials;
            _hashingProvider = hashingProvider;
            _cryptographyProvider = cryptographyProvider;
            _messagePackService = messagePackService;
        }
    }
}

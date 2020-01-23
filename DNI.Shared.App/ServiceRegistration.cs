using DNI.Shared.Contracts;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using DNI.Shared.Services.Extensions;
using DNI.Shared.App.Domains;
using Microsoft.EntityFrameworkCore;

namespace DNI.Shared.App
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services
                .AddDbContextPool<TestDbContext>(options => options.UseSqlServer("Server=localhost;Database=KeyExchange;Trusted_Connection=true"))
                .RegisterDbContentRepositories<TestDbContext>(ServiceLifetime.Transient, typeof(Customer))
                .RegisterCryptographicCredentials<MCryptographicCredentials>(KeyDerivationPrf.HMACSHA512, Encoding.ASCII, 
                "drrNR2mQjfRpKbuN9f9dSwBP2MAfVCPS", 
                "vaTfUcv4dK6wYF6Z8HnYGuHQME3PWWYnz5VRaJDXDSPvFWJxqF2Q2ettcbufQbz5", 1000000, 32, null);;
        }
    }
}

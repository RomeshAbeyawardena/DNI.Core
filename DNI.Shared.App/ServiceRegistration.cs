using DNI.Shared.Contracts;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using DNI.Shared.Shared.Extensions;
using System.Text;
using DNI.Shared.Services.Extensions;

namespace DNI.Shared.App
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.RegisterCryptographicCredentials<MCryptographicCredentials>(KeyDerivationPrf.HMACSHA512, Encoding.ASCII, "drrNR2mQjfRpKbuN9f9dSwBP2MAfVCPS", "vaTfUcv4dK6wYF6Z8HnYGuHQME3PWWYnz5VRaJDXDSPvFWJxqF2Q2ettcbufQbz5", 1000000, 32, null);;
        }
    }
}

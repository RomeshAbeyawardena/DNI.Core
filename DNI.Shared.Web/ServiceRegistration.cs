using AutoMapper;
using DNI.Shared.Contracts.Options;
using DNI.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using DNI.Shared.Web.Contracts;
using DNI.Shared.Web.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using DNI.Shared.App;
using DNI.Shared.Shared.Extensions;
using DNI.Shared.Services.Extensions;
using System;

namespace DNI.Shared.Web
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services, IServiceRegistrationOptions options)
        {
            var currentAssembly = Assembly.GetAssembly(typeof(ServiceRegistration));

            services
                .AddTransient<IPageService, PageService>()
                .AddSingleton<ExceptionHandler>()
                .AddAutoMapper(currentAssembly)
                  .RegisterCryptographicCredentialsFactory<MCryptographicCredentials>((factory, cryptographyProvider, s) => factory
                .CaseWhen("PersonalDataEncryption", cryptographyProvider
                .GetCryptographicCredentials<MCryptographicCredentials>(KeyDerivationPrf.HMACSHA512,
                    Encoding.UTF8, "3d21cecb-189d-4e6b-bea1-91b68de3a37b", "851a5944-115f-4e79-b468-82b67f00e349", 1000000, 32, "851a5944-115f-4e".GetBytes(Encoding.UTF8)))
                .CaseWhen("IdentifierDataEncryption", cryptographyProvider
                    .GetCryptographicCredentials<MCryptographicCredentials>(KeyDerivationPrf.HMACSHA512,
                    Encoding.UTF8, "42e6f1f0-7cd2-4ce3-a06c-f86c1c82fd24", "eeaf5b47-636c-4997-ae41-d979e3b04094", 1000000, 32, "bceac9fa-70a3-4b".GetBytes(Encoding.UTF8))))
                .RegisterCryptographicCredentials<MCryptographicCredentials>(KeyDerivationPrf.HMACSHA512, Encoding.ASCII,
                "drrNR2mQjfRpKbuN9f9dSwBP2MAfVCPS",
                "vaTfUcv4dK6wYF6Z8HnYGuHQME3PWWYnz5VRaJDXDSPvFWJxqF2Q2ettcbufQbz5", 1000000, 32, null)
                .RegisterExceptionHandlers();
            services.AddMediatR(currentAssembly);
        }
    }
}

using AutoMapper;
using DNI.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using DNI.Shared.Web.Contracts;
using DNI.Shared.Web.Services;

namespace DNI.Shared.Web
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services, IServiceRegistrationOptions options)
        {
            var currentAssembly = Assembly.GetAssembly(typeof(ServiceRegistration));

            services
                .AddTransient<IPageService, PageService>()
                .AddAutoMapper(currentAssembly);

            services.AddMediatR(currentAssembly);
        }
    }
}

using AutoMapper;
using DNI.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace DNI.Shared.Web
{
    public class ServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            var currentAssembly = Assembly.GetAssembly(typeof(ServiceRegistration));

            services
                .AddAutoMapper(currentAssembly);

            services.AddMediatR(currentAssembly);
        }
    }
}

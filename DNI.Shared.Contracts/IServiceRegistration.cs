using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts
{
    public interface IServiceRegistration
    {
        void RegisterServices(IServiceCollection serviceCollection);
    }
}

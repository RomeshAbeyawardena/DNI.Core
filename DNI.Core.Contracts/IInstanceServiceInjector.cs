using System;

namespace DNI.Core.Contracts
{
    public interface IInstanceServiceInjector
    {
        object CreateInstance(Type serviceType);
        TService CreateInstance<TService>();
    }
}
